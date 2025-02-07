using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour
{
    [SerializeField] private Transform lugarVazio = null; //define o lugarvazio
    private Camera _camera; //define a camere
    [SerializeField] private TileScript[] tiles; //define as pecas
    private int lugarVazioIndex = 8; //define o lugar do lugar vazio
    private TileScript tileSelecionado = null; //pega o script das pecas
    private Vector3 startDragPosition; //define a posicao do arrastamento
    private Vector3 dragDirection; //define a direcao do arrastamento
    [SerializeField] private GameObject victoryPanel; // define o painel de vitória

    void Start()
    {
        _camera = Camera.main;
        victoryPanel.SetActive(false); // esconde o painel no início
        Embaralhar(); //ativa a funcao para embaralhar as peças no comeco
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction); 
            if (hit)
            {
                TileScript thisTile = hit.transform.GetComponent<TileScript>();

                if (thisTile != null && PodeMover(thisTile)) //ve se pode mover a peca
                {
                    tileSelecionado = thisTile;
                    startDragPosition = thisTile.transform.position;

                    if (Mathf.Abs(lugarVazio.position.x - tileSelecionado.transform.position.x) < 0.1f)
                    {
                        dragDirection = Vector3.up;
                    }
                    else if (Mathf.Abs(lugarVazio.position.y - tileSelecionado.transform.position.y) < 0.1f)
                    {
                        dragDirection = Vector3.right;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && tileSelecionado != null)
        {
            if (PodeMover(tileSelecionado) && Vector2.Distance(tileSelecionado.transform.position, lugarVazio.position) < 1.5f) //inverte os 2
            {
                TrocarPosicao(tileSelecionado);

                if (VerificarVitoria())
                {
                    victoryPanel.SetActive(true);
                }
            }
            else
            {
                tileSelecionado.transform.position = startDragPosition;
            }

            tileSelecionado = null;
        }

        if (tileSelecionado != null)
        {
            Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            Vector3 novaPosicao = startDragPosition;

            if (dragDirection == Vector3.right)
            {
                float minX = Mathf.Min(startDragPosition.x, lugarVazio.position.x);
                float maxX = Mathf.Max(startDragPosition.x, lugarVazio.position.x);
                novaPosicao.x = Mathf.Clamp(mousePosition.x, minX, maxX);
            }
            else if (dragDirection == Vector3.up)
            {
                float minY = Mathf.Min(startDragPosition.y, lugarVazio.position.y);
                float maxY = Mathf.Max(startDragPosition.y, lugarVazio.position.y);
                novaPosicao.y = Mathf.Clamp(mousePosition.y, minY, maxY);
            }

            tileSelecionado.transform.position = novaPosicao;
        }
    }

    private bool PodeMover(TileScript tile)
    {
        return Vector2.Distance(lugarVazio.position, tile.transform.position) < 4;
    }

    private void TrocarPosicao(TileScript tile)
    {
        Vector2 lastEmptySpacePosition = lugarVazio.position;
        lugarVazio.position = tile.targetPosition;
        tile.targetPosition = lastEmptySpacePosition;

        int tileIndex = EncontrarIndex(tile);
        tiles[lugarVazioIndex] = tiles[tileIndex];
        tiles[tileIndex] = null;
        lugarVazioIndex = tileIndex;
    }

    private bool VerificarVitoria() //verifica se as pecas estao no lugar certo
    {
        foreach (var tile in tiles)
        {
            if (tile != null && !tile.EstaNoLugarCorreto())
            {
                return false;
            }
        }
        return true;
    }

    private void Embaralhar()
    {
        // garante que a peça 2 inicie no lugar da peça 8
        int indexPeca2 = EncontrarPecaPorNumero(2);
        if (indexPeca2 != -1 && indexPeca2 != 7)
        {
            // troca a posição da peça 2 com a posição da peça 8 
            Vector3 tempPos = tiles[7].targetPosition;
            tiles[7].targetPosition = tiles[indexPeca2].targetPosition;
            tiles[indexPeca2].targetPosition = tempPos;

            // atualiza a lista 
            TileScript tempTile = tiles[7];
            tiles[7] = tiles[indexPeca2];
            tiles[indexPeca2] = tempTile;
        }

        int inversions;
        do
        {
            List<int> indicesDisponiveis = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
            Vector3[] posicoesIniciais = new Vector3[tiles.Length];

            for (int i = 0; i < indicesDisponiveis.Count; i++)
            {
                posicoesIniciais[i] = tiles[i].targetPosition;
            }

            for (int i = 0; i < indicesDisponiveis.Count; i++)
            {
                int randomIndex = Random.Range(i, indicesDisponiveis.Count);
                var temp = indicesDisponiveis[i];
                indicesDisponiveis[i] = indicesDisponiveis[randomIndex];
                indicesDisponiveis[randomIndex] = temp;
            }

            for (int i = 0; i < indicesDisponiveis.Count; i++)
            {
                tiles[i].targetPosition = posicoesIniciais[indicesDisponiveis[i]];
                TileScript tempTile = tiles[i];
                tiles[i] = tiles[indicesDisponiveis[i]];
                tiles[indicesDisponiveis[i]] = tempTile;
            }

            inversions = PegarInversions();
        } while (inversions % 2 != 0);
    }

    // função auxiliar para encontrar a peça pelo número
    private int EncontrarPecaPorNumero(int numero)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i] != null && tiles[i].number == numero)
            {
                return i;
            }
        }
        return -1;
    }


    private int EncontrarIndex(TileScript ts)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i] != null && tiles[i] == ts)
            {
                return i;
            }
        }
        return -1;
    }

    private int PegarInversions()
    {
        int inversionsSum = 0;
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i] == null) continue;

            int thisTileInversion = 0;
            for (int j = i + 1; j < tiles.Length; j++)
            {
                if (tiles[j] != null && tiles[i].number > tiles[j].number)
                {
                    thisTileInversion++;
                }
            }
            inversionsSum += thisTileInversion;
        }
        return inversionsSum;
    }

    public void VoltarAoMenu()
    {
        SceneManager.LoadScene("MenuPrincipal"); //volta ao menu
    }
}
