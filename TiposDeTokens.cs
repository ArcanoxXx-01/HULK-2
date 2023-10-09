namespace HULK;

public enum TokenType
{
    //Operadores aritmeticos
    suma,
    resta,
    multiplicacion,
    division,
    modulo,
    Pow,
    Concatenar,
    //Operadores de comparacion
    mayor,
    mayorIgual,
    menor,
    menorIgual,
    dobleIgual,
    desigual,
    //Booleanos
    True,
    False,
    negacion,
    or,
    and,
    //Variables
    String,
    numero, 
    //Otros
    identificador,
    print,
    funtion,
    Let,
    In,
    PI,
    Euler,
    //asignar
    igual,
    flechita,
    //separadores
    coma,
    puntoYcoma,
    parentesisAbierto,
    parentesisCerrado,
}