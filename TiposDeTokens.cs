namespace HULK;

public enum TokenType
{
    //Operadores aritmeticos
    suma,
    resta,
    multiplicacion,
    division,
    modulo,
    //Operadores de comparacion
    mayor,
    mayorIgual,
    menor,
    menorIgual,
    dobleIgual,
    desigual,
    //Tipos de variables
    String,
    Number,
    Bool,
    //Otros
    identificador,
    numero,
    negacion,
    igual,
    flechita,
    coma,
    puntoYcoma,
    parentesisAbierto,
    parentesisCerrado,
}