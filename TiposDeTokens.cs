namespace HULK;
    public enum TokenType
    {
        //Operadores aritmeticos
        Suma,
        Resta,
        Multiplicacion,
        Division,
        Pow,
        Modulo,
        Concatenar,

        //Operadores Booleanos Normales
        And,
        Or,
        Negacion,

        //Operadores Booleanos de Comparacion
        IgualIgual,
        NoIgual,
        Mayor,
        MayorIgual,
        Menor,
        MenorIgual,
        
        //Variables
        String,
        Number,
        True,
        False,
        Identificador,
        
        //Asignacion
        Igual,        
        Flechita,

        //Palabras Reservadas
        If,
        Else,
        function,
        Let,
        In,
        PI,
        EULER,

        //Otros
        Coma,
        PuntoYComa,
        ParentesisAbierto,
        ParentesisCerrado,

        //Fin de la Linea
        Final,
    }