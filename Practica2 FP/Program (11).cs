using System.Data;

namespace Kakurasu;

public class Program {    
    static void Main() {    
        Console.CursorVisible = false;

        int[,] ex1 = new int[,] {
            { 0, 0, 0, 4}, // última col: suma por filas
            { 0, 0, 0, 5},
            { 0, 0, 0, 0},
            { 1, 2, 3, 0}  // ultima fil: suma por cols; el último 0 no cuenta
        };
        int[,] ex2 = new int[,] {
            { 0, 0, 0, 0, 7 },
            { 0, 0, 0, 0, 8 },
            { 0, 0, 0, 0, 5 },
            { 0, 0, 0, 0, 6 },
            { 3, 8, 5, 7, 0 }
        };

        // seleccion de ejemplo        
        int[,] mat = ex1;

        int N = mat.GetLength(0) - 1;

        // inicialización y renderizado inicial
        char[,] tab = new char[N, N];
        int[] objFil = new int[N];
        int[] objCol = new int[N];
        Inicializa(mat, ref tab, ref objFil, ref objCol, out int fil, out int col);
        Render(tab, objFil, objCol, fil, col);

        // bucle ppal
        bool parar = false;
        char input;
        while (!parar)
        {

            Render(tab, objFil, objCol, fil, col);
            input=LeeInput();
            ProcesaInput(input, ref fil, ref col, ref tab);

            if (input == 'q')
            {
                parar = true;
            }
            else
            {
                if (Terminado(tab, objFil, objCol))
                {
                    parar=true;
                }
            }
        }
        // informee final

    }



    static char LeeInput() {
        char d = ' ';
        string tecla = Console.ReadKey(true).Key.ToString ();
		switch (tecla) {
		    case "LeftArrow":  d = 'l'; break;
		    case "UpArrow":    d = 'u'; break;
		    case "RightArrow": d = 'r'; break;
		    case "DownArrow":  d = 'd';	break;
            case "X":          d = 'x'; break;  // marcar casilla
            case "V":          d = 'v'; break;  // marcar casilla vacia
            case "C":          d = 'c'; break;  // comprobar incorrectas
            case "Spacebar":   d = 's'; break;  // limpiar casilla
            case "Escape":     d = 'q'; break;  // terminar
            default:           d = ' '; break;
		}   
		return d;
	}
    static void Inicializa(int[,] mat, ref char[,] tab, ref int[] obFil, ref int[] obCol, out int fil, out int col)
    {
        int N = mat.GetLength(0) - 1;

        fil = 2;
        col = 2;

        //Da valores a la matriz tab
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                switch (mat[i, j])
                {
                    case 1:
                        tab[i, j] = 'X';
                        break;
                    case 2:
                        tab[i, j] = '.';
                        break;
                    case 0:
                    default:
                        tab[i, j] = ' ';
                        break;
                }
            }
        }

        //Da valores a obCol y obFil
        for (int i = 0; i < N; i++)
        {
            obFil[i] = mat[i, N];  // Última columna
        }

        for (int j = 0; j < N; j++)
        {
            obCol[j] = mat[N, j];  // Última fila
        }
    }
    static void Render(char[,] tab, int[] objFil, int[] objCol, int fil, int col)
    {
        Console.Clear();
        //MUESTRA EL TABLERO COMPLETO

        //Escribe el numero de las columnas
        Console.Write("   ");
        for(int i=0; i<tab.GetLength(0);i++)
        {
            int n = i + 1;
            Console.Write(" "+n);
        }

        //Escribe la linea entre el tablero y las columnas
            Console.WriteLine(" ");
            Console.Write("   ");
        for (int i = 0; i < tab.GetLength(0); i++)
        {
            Console.Write("__");
        }

        //Escribe las filas y objFil
        
        for(int i = 0; i < tab.GetLength(0); i++)
        {
            int n= i + 1;
            Console.Write("\n"+n+ " | ");
            for(int j = 0;j < tab.GetLength(0); j++)
            {
                Console.Write(tab[i,j]+" ");
            }
            Console.Write("| " + objFil[i]);
        }

        //Escribe la linea entre el tablero y las columnas
        Console.WriteLine(" ");
        Console.Write("   ");
        for (int i = 0; i < tab.GetLength(0); i++)
        {
            Console.Write("--");
        }

        //Escribe ObjCol
        Console.WriteLine(" ");
        Console.Write("    ");
        for (int i = 0; i < tab.GetLength(0); i++)
        {
            int n = i + 1;
            Console.Write(objCol[i]+" ");
        }

        //MUESTRA EL CURSOR
        Console.SetCursorPosition(col*2, fil);
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.Green;
        Console.WriteLine(" ");
        Console.BackgroundColor = ConsoleColor.Black;
    }
    static void ProcesaInput(char tecla, ref int fil, ref int col, ref char[,] tab)
    {
        //Límites por donde se puede mover el cursor
        int maxUp = 2,
            maxDown = tab.GetLength(0) + 1,
            maxLeft = 2,
            maxRight = tab.GetLength(0) + 1;

        //Posicion real del cursor en tab
        int filTab = fil - 2,
            colTab = col - 2;

        switch (tecla)
        {
            case 'u':
                if(fil>maxUp)
                fil--;
                break;
            case 'd':
                if(fil<maxDown)
                fil++;
                break;
            case 'l':
                if (col>maxLeft)
                col--;
                break;
            case 'r':
                if (col<maxRight)
                col++;
                break;
            case 'x':
                tab[filTab, colTab] = 'X';
                break;
            case 'v':
                tab[filTab, colTab] = '.';
                break;
            case 's':
                tab[filTab, colTab] = ' ';
                break;
        }
    }
    static int SumaFil(char[,] tab,  int fil)
    {
        int sumaFil = 0;
        for(int i=0; i < tab.GetLength(0); i++)
        {
            if (tab[fil,i] == 'X')
                sumaFil= sumaFil + (i+1);
        }
        return sumaFil;
    }

    static int SumaCol(char[,] tab,  int col)
    {
        int sumaCol = 0;
        for (int i = 0; i < tab.GetLength(0); i++)
        {
            if (tab[i, col] == 'X')
                sumaCol=sumaCol + (i + 1);
        }
        return sumaCol;
    }

    static bool Terminado(char[,] tab, int[] objFil, int[] objCol)
    {
        bool terminado = false;
        bool estamal=false;
        int i= 0;
        while (!estamal)
        {
            if (i == tab.GetLength(0))
            {
                terminado = true;
                estamal = true;
            }
            else
            {
                if (SumaCol(tab, i) != objCol[i] || SumaFil(tab, i) != objFil[i])
                {
                    estamal = true;
                    terminado = false;
                }
                else
                {
                    i++;
                }
            }
        }
        return terminado;
    }
}
