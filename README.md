For the current status of the program, more information, downloads and documentation:
<http://arvehansen.net/codecave/>

## ACopy
A tool for copying a complete database to/from text files. It creates schema files and data files per table. The text files can be copied into either an Oracle, Sql Server (or Sql Azure) database. This means that you can copy a complete database from Oracle to Sql Server (or vise versa). It should:

* Handle conversion from Oracle data types to SQL Server data types (and vise versa).
* Handle column constraints (default values and null values)
* Handle primary/foreign keys
* Handle indexes
* Handle views (could be limitations if copying from one RDBMS to another)
* Handle Procedures (could be limitations if copying from one RDBMS to another)

Since the data files are plain text files (comma separated values), it can be used to get data for other tools that does not support connecting to a database. It can also be used to backup a database.

## Background

I work with an ERP system called Unit4 Business World (previously called Agresso). Through the years, the ERP system has been using many different databases to store its data. In the 90's I wrote a program to transfer both data and schema between these databases. Doing this manually is a pain, since the data types and DDL (Database Definition Language) for the different RDBMS's are different. In the beginning, this program was very slow. It was improved upon using different bulk operations, but still pretty slow in certain cases.

Improving this tool never really got any priority. It was also written in C++, using pretty low-level database libraries (e.g. OCI for Oracle and blk for SQL Server), so difficult to maintain.

A few years ago, my boss and friend Anders Hauan tried out the .NET database libraries for SQL Server and Oracle. By writing a custom DataReader and using the .NET parallel library, he could move data much faster than my copy program. He wanted me to take over the code, but as I often find, it's faster to start from scratch (sorry Anders :-) )

So I wrote a copy program that used these ideas, and managed to copy a complete UBW database (Unit4 Business World is an ERP program) between Oracle, SQL Server, and SQL Azure.

## Current state

The program was (and is still) done on my spare time. It's been more or less untouched for the last two years (2014-2016). It currently only handles the data types used by UBW (Unit4 Business World is an ERP program), and there is some special handling for indexes and views. It also does some special type conversions (like char will always be converted to varchar, number(30,3) to money, etc). This will be changed so conversions can be dynamic, and all the special UBW handling will be optional.