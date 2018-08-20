using System;
using System.Collections.Generic;
using DataTransferObjects;
using MySql.Data.MySqlClient;


namespace DataAccessObjects
{
    public class WorkflowDAO: DataAccessBase
    {
        public WorkflowDAO(MySqlConnection mySqlConnection)
        {
            this.mySqlConnection = mySqlConnection;
        }

        public List<WorkflowDTO> RetrieveWorkflows(String filter)
        {
            List<WorkflowDTO> workflowList = new List<WorkflowDTO>();

            String query = "SELECT * FROM `flowManager`.`workflow`";
            MySqlCommand command = new MySqlCommand(query, this.mySqlConnection);
            MySqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                WorkflowDTO workflow = new WorkflowDTO();
                workflow.id = (int)dataReader["id"];
                workflow.name = (String)dataReader["name"];
                workflow.startElement = (int)dataReader["startElement"];
                workflow.finishElement = (int)dataReader["finishElement"];

                workflowList.Add(workflow);
            }
            dataReader.Close();

            return workflowList;
        }

        public Int64 StoreWorkflow(WorkflowDTO workflow)
        {
            String commandText = "INSERT INTO `flowManager`.`workflow` VALUES(null, '" + workflow.name + "', " + workflow.startElement + ", " + workflow.finishElement + ")";
            if (workflow.id != 0) commandText = "UPDATE `flowManager`.`workflow` SET name = '" + workflow.name + "', startElement = " + workflow.startElement + ", finishElement = " + workflow.finishElement + " WHERE id = " + workflow.id;
            commandText = commandText + ';' + Environment.NewLine + "SELECT LAST_INSERT_ID()";

            MySqlCommand command = new MySqlCommand(commandText, this.mySqlConnection);
            Int64 insertId = (Int64)command.ExecuteScalar();
            if (workflow.id != 0) insertId = workflow.id;

            return insertId;
        }
    }

}
