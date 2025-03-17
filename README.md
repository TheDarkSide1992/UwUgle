
<img src="./Frontend/public/favicon.ico" width="50%" height="50%">

# UwUgle
### DEVS
* Jens
* Andreas
* Emil

## Purpose
This Projet is a school compulsery project at EASV(erhvervsakademi sydvest | business academy southwest). \
This project where made for purely educational purposes and should not be used for any monitational gains.


## Running the app

To sart Insert the files you want indexed Into [**`maildir`**](maildir).
After this you can run the program by running
> **`docker compose up`**

In case you want to rebuild the application run the folowing(This might be nesesary after updates)
> **`docker compose up --build`**

In case you want to reset the database you should clear the docker volume **`uwugle_pgdata`**


## Known Issues
 * In some situations the [**`sql-set-up.sh`**](scripts/sql-set-up.sh) might not function correctly. this can be sercomvented by changing the SQL_FOLDER to mtch relevant path for your docker instance
