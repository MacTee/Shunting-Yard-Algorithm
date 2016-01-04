#Shunting-yard with support of unary operation  
  
This project provides a way to evaluate mathematical formulas.  
It is realised by an implementation of the Shunting-yard algorithm ([Wiki article](https://en.wikipedia.org/wiki/Shunting-yard_algorithm)), that converts the infix formula to  
postfix notation (or [RPN - Reverse Polish notation](https://en.wikipedia.org/wiki/Reverse_Polish_notation)) and by evaluating the postfix formula.

The implementation of the Shunting-yard algorithm is slightly adjusted (compared to the Wiki article):
```
- support of unary operations like '-1' or '1+(-2)' added
- support of functions like sqr(9) removed.
```
  
This implementation supports the following operations:
```
- '-' (subtraction)
- '+' (addition)
- '*' (multiplication)
- '/' (division)
- '^' (to the power of)
- '-1' or '+1' (negative/positive numbers)
```

The support of unary operations is realised by replacing the 'additional' plus or minus signs with a 'p' or 'm' in the postfix notation and handle those operations differently during postfix evaluation.
