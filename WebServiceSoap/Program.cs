using WebServiceSoap;

string? op, a, b;

int result = 1;
while(result != 0)
{

    a = Console.ReadLine();
    op = Console.ReadLine();
    b = Console.ReadLine();

    result = op switch
    {
        "+" => CalculatorSoapClient.Calculate(Convert.ToInt32(a), Convert.ToInt32(b), CalculatorOperations.Add),
        "-" => CalculatorSoapClient.Calculate(Convert.ToInt32(a), Convert.ToInt32(b), CalculatorOperations.Subtract),
        "*" => CalculatorSoapClient.Calculate(Convert.ToInt32(a), Convert.ToInt32(b), CalculatorOperations.Multiply),
        "/" => CalculatorSoapClient.Calculate(Convert.ToInt32(a), Convert.ToInt32(b), CalculatorOperations.Divide),
        _ => 0
    } ;

    Console.WriteLine("Resultado: {0}", result);
}