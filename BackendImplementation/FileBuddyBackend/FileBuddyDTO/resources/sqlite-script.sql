/* *****************************************
Description:	This script contains the main structure for the first prototype of the 
				FileBuddy file sharing application database. 
				
Author:			Jessica Veit 
Creation Date:	2020-05-09
Last updated:	2020-05-11
*********************************** */

CREATE TABLE IF NOT EXISTS AppUser (
	id						INTEGER		PRIMARY KEY,
	name 					TEXT 		NOT NULL,
   	password 				TEXT  		NOT NULL,
	mail_address 			TEXT  		NOT NULL UNIQUE,
	account_creation_date	TEXT		NOT NULL,
	profil_picture			BLOB, 
	
	
	user_devices	TEXT, -- JSON, representing a list of user devices
	user_groups		TEXT -- JSON, representing a list of user groups
);

CREATE TABLE IF NOT EXISTS SharedFile (
	id						INTEGER		PRIMARY KEY,
	shared_file_name		TEXT		NOT NULL, 
	api_path				TEXT		NOT NULL, 
	owner_user_id  			INTEGER		NOT NULL, 
	upload_date				TEXT 		NOT NULL,
	
	FOREIGN KEY(owner_user_id) REFERENCES AppUser(id)
);

CREATE TABLE IF NOT EXISTS DownloadTransaction (
	id						INTEGER		PRIMARY KEY,
	receiver_user_id		INTEGER		NOT NULL, 
	shared_file_id			TEXT		NOT NULL, 
	download_date			TEXT		NOT NULL,
	
	FOREIGN KEY(receiver_user_id) REFERENCES AppUser(id), 
	FOREIGN KEY(shared_file_id) REFERENCES SharedFile(id)
);

-- AUTOINCREMENT: INSERT INTO test1(rowid, a, b) VALUES(123, 5, 'hello');