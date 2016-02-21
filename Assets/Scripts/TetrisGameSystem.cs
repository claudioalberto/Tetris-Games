using UnityEngine;
using System.Collections;

//Developer: Claudio Alberto
//Tetris Game System make in unity


public class TetrisGameSystem : MonoBehaviour {

    int[,] gameScreen; //Tela do Jogo
    public int height; //Altura da tela
    public int width; //Largura da Tela do Jogo
    GameObject[,] blocks; //matris para criação dos blocos
    public GameObject blockModel; //Prefab do Bloco Modelo
    public float timeIntervalToMoveBlockToDown; //intervalo de tempo para descer os blocos
    public float timeIntervalToMoveBlockToDownMax; //limite de tempo para o intervalo de tempo
    void Start()
    {
        gameScreen = new int[width, height];
        blocks = new GameObject[width, height];
        DrawScreen();
    }
    //Desenha os blocos na tela 
    void DrawScreen()
    {
        int i, j;
        for (i=0; i < height; i++)
        {
            for (j=0; j < width; j++)
            {
                gameScreen[j, i] = -1;
                blocks[j, i] = Instantiate(blockModel, new Vector2(j+(j*0.1f), i+(i*0.1f)), Quaternion.identity) as GameObject;
            }
        }
        
        gameScreen[5, 10] = 1;
        gameScreen[5, 11] = 1;
        gameScreen[4, 10] = 1;
        gameScreen[6, 10] = 1;
    }
    //Colore os blocks da Tela
    void SetColorInBlocks()
    {
        int i, j;
        for (i = 0; i < height; i++)
        {
            for (j = 0; j < width; j++)
            {
                if (gameScreen[j, i] == -1)
                    blocks[j, i].gameObject.GetComponent<Renderer>().material.color = Color.gray;
                if (gameScreen[j, i] == 0)
                    blocks[j, i].gameObject.GetComponent<Renderer>().material.color = Color.white;
                if (gameScreen[j, i] == 1)
                    blocks[j, i].gameObject.GetComponent<Renderer>().material.color = Color.blue;
            }
        }
    }
    //Faz os blocos descerem
    void MoveBlockToDown()
    {
        bool OK = MoveBlockToDownOK();
        int i, j;
        for(i=0; i < height; i++)
        {
            for(j = 0; j < width; j++)
            {
                if (gameScreen[j, i] == 1)
                {
                    if (OK)
                    {
                        gameScreen[j, i] = -1;
                        gameScreen[j, i - 1] = 1;
                    }
                    else{ gameScreen[j, i] = 0; }
                    
                }
            }
        }
    }
    //Verifica se os blocos podem descer ou não
    bool MoveBlockToDownOK()
    {
        int i, j;
        for (i = 0; i < height; i++)
        {
            for (j = 0; j < width; j++)
            {
                if (gameScreen[j, i] == 1)
                {
                    if (i > 0)
                    {
                        if (gameScreen[j, i - 1] == 0)
                        {
                            return false;
                        }
                    }
                    else { return false; }
                        
                }
            }
        }
        return true;
    }
    //verifica se é possivel mover os blocos
    bool MoveBlockHorizontalOK(int direction)
    { 
        int i, j;
        int initialJ;
        if (direction == -1)
        {
            initialJ = 0;
        }
        else { initialJ = width-1; }
        for (i = 0; i < height; i++)
        {
            j = initialJ;
            while (j >= 0 && j < width)
            {
                if (gameScreen[j,i]==1)
                {
                    if ((j + direction) < 0 || j >= width)
                    {
                        return false;
                    }
                    else
                    {
                        if (gameScreen[j + direction, i] == 0)
                            return false;
                    }
                    
                }
                
            }
            j = j - direction;
        }
        return true;
    }
    //Move os blocos horizontalmente
    void MoveBlockHorizontal(int direction)
    {
        int i, j;
        int initialJ;
        bool OK = MoveBlockHorizontalOK(direction);
        if (direction == -1)
        {
            initialJ = 0;
        }
        else { initialJ = width - 1; }
        for (i = 0; i < height; i++)
        {
            j = initialJ;
            while (j >= 0 && j < width)
            {
                if (gameScreen[j, i] == 1)
                {
                    if(OK)
                    {
                        gameScreen[j, i] = -1;
                        gameScreen[j + direction, i] = 1;
                    }
                }
                j = j - direction;
            }
            
        }
        
    }

    void Controller()
    {
        if(Input.GetKeyDown(KeyCode.A)){
            MoveBlockHorizontal(-1);
        }
        if (Input.GetKeyDown(KeyCode.D)){
            MoveBlockHorizontal(1);
        }
    }
    void Update()
    {
        timeIntervalToMoveBlockToDown += Time.deltaTime;
        if(timeIntervalToMoveBlockToDown > timeIntervalToMoveBlockToDownMax)
        {
            MoveBlockToDown();
            timeIntervalToMoveBlockToDown = 0;
        }
        SetColorInBlocks();
        Controller();
    }
}
