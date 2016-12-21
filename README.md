For the current status of the program, more information, downloads and documentation:
<http://arvehansen.net/codecave/>

# ACopy
A tool for copying a complete database to/from text files. It creates schema files and data files per table. The text files can be copied into either an Oracle, Sql Server (or Sql Azure) database. This means that you can copy a complete database from Oracle to Sql Server (or vise versa). It should:

* Handle conversion from Oracle data types to SQL Server data types (and vise versa).
* Handle column constraints (default values and null values)
* Handle primary/foreign keys
* Handle indexes
* Handle views (could be limitations if copying from one RDBMS to another)
* Handle Procedures (could be limitations if copying from one RDBMS to another)

Since the data files are plain text files (comma separated values), it can be used to get data for other tools that does not support connecting to a database. It can also be used to backup a database.


