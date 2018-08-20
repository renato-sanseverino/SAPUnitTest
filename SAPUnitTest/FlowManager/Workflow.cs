using System;
using System.Collections.Generic;
using ClassLibrary;
using DataAccessObjects;
using DataTransferObjects;


namespace FlowManager
{
    public class Workflow
    {
        private int id;
        private String name;
        private List<FlowElement> elements;
        private List<Relation> relations;
        private FlowElement startElement;
        private FlowElement finishElement;


        public Workflow(String name)
        {
            this.name = name;
            this.elements = new List<FlowElement>();
            this.relations = new List<Relation>();
        }

        public void AddElement(FlowElement element)
        {
            elements.Add(element);
        }

        public void CreateRelation(FlowElement origin, FlowElement destination)
        {
            Relation relation = new Relation();
            relation.origin = origin;
            relation.destination = destination;

            relations.Add(relation);
        }

        public List<FlowElement> GetPreviousElements(FlowElement element)
        {
            List<FlowElement> previousElements = new List<FlowElement>();

            foreach (Relation relation in relations)
            {
                if (relation.destination == element)
                    previousElements.Add(relation.origin);
            }

            return previousElements;
        }

        public List<FlowElement> GetNextElements(FlowElement element)
        {
            List<FlowElement> nextElements = new List<FlowElement>();

            foreach (Relation relation in relations)
            {
                if (relation.origin == element)
                    nextElements.Add(relation.destination);
            }

            return nextElements;
        }

        public void Store()
        {
            DataConnector dataConnector = new DataConnector();
            dataConnector.OpenConnection("mySql");

            WorkflowDAO workflowDAO = new WorkflowDAO(dataConnector.MySqlConnection);
            ElementDAO elementDAO = new ElementDAO(dataConnector.MySqlConnection);
            RelationDAO relationDAO = new RelationDAO(dataConnector.MySqlConnection);

            WorkflowDTO workflowDTO = new WorkflowDTO();
            workflowDTO.id = this.id;
            workflowDTO.name = this.name;
            if (this.startElement != null)
                workflowDTO.startElement = this.startElement.id;
            if (this.finishElement != null)
                workflowDTO.finishElement = this.finishElement.id;
            this.id = (int)workflowDAO.StoreWorkflow(workflowDTO);

            foreach(FlowElement element in elements)
            {
                ElementDTO elementDTO = new ElementDTO();
                elementDTO.id = element.id;
                elementDTO.owner = this.id;
                elementDTO.name = element.name;
                elementDTO.enabled = element.enabled;
                elementDTO.elementType = element.elementType;

                elementDAO.StoreElement(elementDTO);
            }

            foreach(Relation relation in relations)
            {
                RelationDTO relationDTO = new RelationDTO();
                relationDTO.origin = relation.origin.id;
                relationDTO.destinarion = relation.destination.id;

                relationDAO.StoreRelation(relationDTO);
            }

            dataConnector.CloseConnection();
        }

        public void Load()
        {
            DataConnector dataConnector = new DataConnector();
            dataConnector.OpenConnection("mySql");

            WorkflowDAO workflowDAO = new WorkflowDAO(dataConnector.MySqlConnection);
            ElementDAO elementDAO = new ElementDAO(dataConnector.MySqlConnection);
            RelationDAO relationDAO = new RelationDAO(dataConnector.MySqlConnection);

            dataConnector.CloseConnection();
        }
    }

}
