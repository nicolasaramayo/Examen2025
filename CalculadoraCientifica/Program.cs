using CalculadoraCientifica.Services;

var calculadora = new CalculadoraService();
bool continuar = true;

while (continuar)
{
    Console.Clear();
    Console.WriteLine("=== Calculadora Científica ===");
    Console.WriteLine("1. Suma");
    Console.WriteLine("2. Resta");
    Console.WriteLine("3. Multiplicación");
    Console.WriteLine("4. División");
    Console.WriteLine("5. Raíz cuadrada");
    Console.WriteLine("6. Potencia");
    Console.WriteLine("7. Logaritmo base 10");
    Console.WriteLine("8. Logaritmo natural");
    Console.WriteLine("9. Factorial");
    Console.WriteLine("0. Salir");
    Console.WriteLine("===========================");

    Console.Write("\nSeleccione una operación: ");
    var opcion = Console.ReadLine();

    try
    {
        switch (opcion)
        {
            case "1":
                RealizarOperacionBinaria("Suma", (a, b) => calculadora.Sumar(a, b));
                break;
            case "2":
                RealizarOperacionBinaria("Resta", (a, b) => calculadora.Restar(a, b));
                break;
            case "3":
                RealizarOperacionBinaria("Multiplicación", (a, b) => calculadora.Multiplicar(a, b));
                break;
            case "4":
                RealizarOperacionBinaria("División", (a, b) => calculadora.Dividir(a, b));
                break;
            case "5":
                RealizarOperacionUnaria("Raíz cuadrada", n => calculadora.RaizCuadrada(n));
                break;
            case "6":
                RealizarOperacionPotencia(calculadora);
                break;
            case "7":
                RealizarOperacionUnaria("Logaritmo base 10", n => calculadora.LogBase10(n));
                break;
            case "8":
                RealizarOperacionUnaria("Logaritmo natural", n => calculadora.LogNatural(n));
                break;
            case "9":
                RealizarOperacionFactorial(calculadora);
                break;
            case "0":
                continuar = false;
                break;
            default:
                Console.WriteLine("\nOpción no válida.");
                break;
        }

        if (continuar)
        {
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\nError: {ex.Message}");
        Console.WriteLine("\nPresione cualquier tecla para continuar...");
        Console.ReadKey();
    }
}

#region Funciones auxiliares

static void RealizarOperacionBinaria(string operacion, Func<double, double, double> func)
{
    Console.WriteLine($"\nOperación: {operacion}");
    Console.Write("Ingrese el primer número: ");
    double num1 = double.Parse(Console.ReadLine()!);
    Console.Write("Ingrese el segundo número: ");
    double num2 = double.Parse(Console.ReadLine()!);

    double resultado = func(num1, num2);
    Console.WriteLine($"\nResultado: {resultado}");
}

static void RealizarOperacionUnaria(string operacion, Func<double, double> func)
{
    Console.WriteLine($"\nOperación: {operacion}");
    Console.Write("Ingrese el número: ");
    double num = double.Parse(Console.ReadLine()!);

    double resultado = func(num);
    Console.WriteLine($"\nResultado: {resultado}");
}

static void RealizarOperacionPotencia(CalculadoraService calculadora)
{
    Console.WriteLine("\nOperación: Potencia");
    Console.Write("Ingrese la base: ");
    double baseNum = double.Parse(Console.ReadLine()!);
    Console.Write("Ingrese el exponente: ");
    double exponente = double.Parse(Console.ReadLine()!);

    double resultado = calculadora.Potencia(baseNum, exponente);
    Console.WriteLine($"\nResultado: {resultado}");
}

static void RealizarOperacionFactorial(CalculadoraService calculadora)
{
    Console.WriteLine("\nOperación: Factorial");
    Console.Write("Ingrese un número entero positivo: ");
    int num = int.Parse(Console.ReadLine()!);

    double resultado = calculadora.Factorial(num);
    Console.WriteLine($"\nResultado: {resultado}");
}

//Console.WriteLine("Hello, World!");

#endregion
