using System;
using System.Collections.Generic;
using DataTransferObjects;
using MySql.Data.MySqlClient;


namespace DataAccessObjects
{
    public class ElementDAO: DataAccessBase
    {
        public ElementDAO(MySqlConnection mySqlConnection)
        {
            this.mySqlConnection = mySqlConnection;
        }

        // Recupera os objetos "element" pertencentes ao workflow especificado
        public List<ElementDTO> RetrieveElements(int workflowId)
        {
            List<ElementDTO> elementList = new List<ElementDTO>();

            String query = "SELECT * FROM `flowManager`.`element` WHERE owner=" + workflowId;
            MySqlCommand command = new MySqlCommand(query, this.mySqlConnection);
            MySqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                ElementDTO element = new ElementDTO();
                element.id = (int)dataReader["id"];
                element.owner = (int)dataReader["owner"];
                element.name = (String)dataReader["name"];
                element.enabled = (Boolean)dataReader["enabled"];
                element.elementType = (String)dataReader["elementType"];
                element.left = (int)dataReader["left"];
                element.top = (int)dataReader["top"];
                element.width = (int)dataReader["width"];
                element.height = (int)dataReader["height"];

                elementList.Add(element);
            }
            dataReader.Close();

            return elementList;
        }

        public Int64 StoreElement(ElementDTO element)
        {
            String commandText = "INSERT INTO `flowManager`.`element` VALUES(null, " + element.owner + ", '" + element.name + "', " + element.enabled + ", '" + element.elementType + "', " + element.left + ", " + element.top + ", " + element.width + ", " + element.height + ")";
            if (element.id != 0) commandText = "UPDATE `flowManager`.`element` SET owner = " + element.owner + ", name = '" + element.name + "',enabled = " + element.enabled + ",  elementType = '" + element.elementType + "', `left` = " + element.left + ", top = " + element.top + ", width = " + element.width + ", height = " + element.height + " WHERE id = " + element.id;
            commandText = commandText + ';' + Environment.NewLine + "SELECT LAST_INSERT_ID()";

            MySqlCommand command = new MySqlCommand(commandText, this.mySqlConnection);
            Int64 insertId = (Int64)command.ExecuteScalar();
            if (element.id != 0) insertId = element.id;

            return insertId;
        }
    }

}
