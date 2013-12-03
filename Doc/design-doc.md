Design Document
===============

UML Diagram
-----------
![diagram](https://raw.github.com/muzzi11/cargo/master/Doc/diagram.png)


Implementation details
----------------------

The UML diagram above describes all the classes and relations that are required for this project. The open-headed arrows represent dependencies while the white-headed arrows represent inheritance.

----------

GUI


 - The UI will be sized for a common resolution (i.e., 320×480 points) and will scale well with higher resolutions.
 - Only portrait mode will be supported.

----------

Data


----------


Style Guide
=======

Style guide for c# programming.

----------

Comments
---------

Not commenting at all is ok when your code can be understood at a glance. Commenting too much indicates bad code.
Only write comments when it is really necessary. Try to write comments that address one or both of these questions:

1. What does this block do?
2. Why did I implement this block in this way?

----------
Inline comments

Within functions, use inline comments and keep them short (e.g., one line), else it becomes
difficult to distinguish comments from code, even with syntax highlighting. Don't forget
to leave one space between the // and your comments first character, as in:

``` c#
// convert Fahrenheit to Celsius
float c = 5.0 / 9.0 * (f - 32.0);
```

----------

Multiline comments

Atop your c# files should be multi-line comments that summarize what your program
(or that particular file) does along with your name:

``` C#
/*
 * Jos Bonsink
 * josbonsink@gmail.com
 *
 * Says hello to the world.
 */
```

Notice how:

 1. The first line start with /* and the last line ends with /* and
 2. all of the asterisks (*) between those lines line up perfectly in a column.

----------

Conditions
----------

Conditions containing a single statement:

For example:
``` c#
if (x > 0)
    foo = 1;
else if (x < 0)
    foo = -1;
else
    foo = x;
```

Notice how:

2. there’s a single space after each if;
3. each call to the function is indented with 4 spaces;
4. there are single spaces around the **>** and around the **>** and
5. there isn't any space immediately after each **(** or immediately before each **)**.


----------
Conditions containing multiple statements:

``` c#
if (x > 0)
{
    Console.write("x is positive\n");
    foo(x);
}
else if (x < 0)
{
    Console.write("x is negative\n");
    bar(x);
}
```

Notice that the curly braces line up nicely, each on its own line, making perfectly clear what’s inside the branch.

----------

Indentation
---------

Indent your code four spaces at a time to make clear which blocks of code are inside of others. Here’s some nicely indented code:

``` c#
public static void main() 
{
  Console.write("Hello world!");
  for (int i= 0; i < 10; i++)
      Console.write("Hello again, times " + i);
}
```

Note, don't use tabs!


----------

Loops
-----

For

Whenever you need temporary variables for iteration, use i, then k, then m, unless more specific names would make your code more readable:

``` c#
for (int i = 0; i < LIMIT; i++)
{
    for (int k = 0; k < LIMIT; j++)
    {
        for (int m = 0; m < LIMIT; m++)
        {
            // do something
        }
    }
}
```

Notice that you shouldn't use j, which is too similar to i.


----------
while

Declare while loops as follows:

``` c#
while (expression) 
{
     statement(s)
}
```


----------
do-while

Declare do-while loops as follows:

``` c#
int count = 1;
do 
{
    Console.write("Count is: " + count);
    count++;
} while (count < 11);
```


----------

Variables
---------

If declaring multiple variables of the same type at once, it's fine to declare them together, as in:

``` c#
int quarters, dimes, nickels, pennies;
```

Overview:

1. Variables should be initialized where they are declared and they should be declared in the smallest scope possible.
2. Variables must never have dual meaning.
3. Variables should be kept alive for as short a time as possible.

----------

Naming conventions
------------------

General naming conventions:

1. Names representing types must be nouns and written in mixed case starting with upper case.
2. Variable names must be in mixed case starting with lower case.
3. Names representing constants (final variables) must be all uppercase using underscore to separate words.
4. Names representing methods must be verbs and written in mixed case starting with lower case.
5. Do not use underscores to indicate class variables.
6. Generic variables should have the same name as their type.
7. All names should be written in English.

Specific Naming Conventions:

1. The terms get/set must be used where an attribute is accessed directly.
2. is prefix should be used for boolean variables and methods.
3. Negated boolean variable names must be avoided.
3. Plural form should be used on names representing a collection of objects.
4. n prefix should be used for variables representing a number of objects.
5. No suffix should be used for variables representing an entity number.
6. Abbreviations in names should be avoided.
7. Associated constants (final variables) should be prefixed by a common type name.
8. Exception classes should be suffixed with Exception.
9. Functions (methods returning an object) should be named after what they return and procedures (void methods) after what they do.