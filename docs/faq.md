# Frequently Asked Questions

/!\ **BEFORE READING** /!\
Before reading the FAQ, read the instructions provided. Reading does not hurt.

## Installation
### Docker related problems

## Database
**Q**: Oh no, I can't connect with my database! I got this message: `[ERROR][DatabasePlugin] Login failed for user 'sa'.`

**A**: This is because the server is trying to connect to the database, but your password is wrong. In this case, you must change your password in files:
* `dist\Debug\Login\plugins\config\DatabasePlugin\conf.json`
* `dist\Debug\World\plugins\config\DatabasePlugin\conf.json`
* Done!

**Q**: [ERROR][DatabasePlugin] A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections.

**A**: It is possible that your database is disabled. To turn it on:
* Run the Command Line
* Enter: `docker start /saltyemu-database`
* Done!