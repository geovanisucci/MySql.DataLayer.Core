using MySql.DataLayer.Core.DatabasePatcher.MigrationTableEntity;
using MySql.DataLayer.Core.DatabasePatcher.Worker;
using System;
using System.Collections.Generic;
using System.IO;

namespace MySql.DataLayer.Core.DatabasePatcher
{
    public class Migration
    {
        MigrationWorker worker;

        /// <summary>
        /// Constructor for Migration to pass connection string and Database Name to the Worker
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        public Migration(string connectionString, string databaseName)
        {
            worker = new MigrationWorker(connectionString, databaseName);
        }

        /// <summary>
        /// Execute all scripts in 'scriptsFolder' if not have a record with same timestamp and filename inside the migrations table
        /// </summary>
        /// <param name="scriptsFolder"></param>
        /// <param name="formatDatabaseName"></param>
        public async void ExecuteMigrations(string scriptsFolder, bool formatDatabaseName = false)
        {
            //Try to create database if doesn't exist
            bool isDatabaseCreated = worker.CreateDatabaseIfNotExist();
            //Try to create migration table if doesn't exist
            bool isMigrationTableCreated = worker.CreateMigrationTableIfNotExist();
           
            if (isDatabaseCreated && isMigrationTableCreated)
            {
                //Return all current records inside the Migration Table
                List<DatabaseMigrationEntity> lstMigrations = await worker.GetCurrentMigrations();
                //Get all information inside DirectoryInfo
                DirectoryInfo d = new DirectoryInfo(scriptsFolder);
                //For each file with .sql extension
                foreach (var sqlScriptFile in d.GetFiles("*.sql"))
                {
                    //Get the Name of Sql Script
                    string fileName = sqlScriptFile.Name;
                    //Create a array removing the extension .sql from the file splitting with the separator _ 
                    string[] fileNameArray = fileName.Replace(".sql", "").Split('_');
                    //Get the TimeStamp based on the last index inside the array
                    string stringFileTimeStamp = fileNameArray[fileNameArray.Length - 1];

                    //Try parse the string from file to a timestamp
                    Int64 fileTimeStamp = 0;
                    Int64.TryParse(stringFileTimeStamp, out fileTimeStamp);

                    //Verify if inside the list of records exists one with the same TimeStamp and File Name
                    if (!lstMigrations.Exists(migr => migr.TimestampMigration == fileTimeStamp 
                                                        && migr.FileName == fileName))
                    {
                        //Read the Sql scripts
                        string scriptQuery = File.ReadAllText(sqlScriptFile.FullName);
                        //Try to execute the script
                        bool isScriptExecuted = worker.ExecuteScriptQuery(scriptQuery, formatDatabaseName);
                        //If is successful insert the TimeStamp and File Name
                        if (isScriptExecuted)
                            worker.AddMigration(fileTimeStamp,fileName);
                    }
                }
            }

            worker = null;            
        }        
    }
}
