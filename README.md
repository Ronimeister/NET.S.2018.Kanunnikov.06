# NET.S.2018.Kanunnikov.06


1. (https://github.com/Ronimeister/NET.S.2018.Kanunnikov.06/tree/master/Extensions)    

    (deadline - 18.00 07.07.2018) Реализовать метод расширения получения из строкового представления целого положительного четырехбайтового числа, записанного в p-ичной системе счисления (2<=p<=16), его десятичного значения (при реализации готовые классы-конверторы не использовать!). Разработать модульные тесты. (NUnit фреймворк). Примерные тесткейсы.

        "0110111101100001100001010111111" для основания 2 -> 934331071
        "01101111011001100001010111111" для основания 2 -> 233620159
        "11101101111011001100001010" для основания 2 -> 62370570
        "1AeF101" для основания 2 -> ArgumentException
        "11111111111111111111111111111111" для основания 2 -> OverflowException
        "1AeF101" для основания 16 -> 28242177
        "1ACB67" для основания 16 -> 1756007
        "SA123" для основания 2 -> ArgumentException
        "764241" для основания 8 -> 256161
        "764241" для основания 20 -> ArgumentException
        "10" для основания 5 -> 5
        и т.д.


2. (https://github.com/Ronimeister/NET.S.2018.Kanunnikov.06/tree/master/PolynomialLib)


    (deadline - 18.00 08.07.2018) Разработать неизменяемый класс Polynomial (полином) для работы с многочленами степени от одной переменной вещественного типа (в качестве внутренней структуры для хранения коэффициентов использовать sz-массив). Для разработанного класса переопределить виртуальные методы класса Object; перегрузить операции, допустимые для работы с многочленами (исключая деление многочлена на многочлен), включая "==" и "!=". Разработать unit-тесты (NUnit фреймворк).
