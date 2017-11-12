<?php
require_once 'models/Record.class.php';
require_once "Collection.class.php";


class PerformanceAPI
{
    protected $recordLimit;
    protected $threads;

    public function __construct($recordLimit, $threads) {
        $this->recordLimit = $recordLimit;
        $this->threads = $threads;
    }

    public function ProcessAPI() {
        $StartTime = microtime(true);

        // Cater for header
        $this->recordLimit = $this->recordLimit - 1;    
        
        $resultSet = new Collection();

        $recordCount = 0;
        $handle = fopen("c:\data\data.csv", "r");
        if ($handle) {
            while ((($line = fgets($handle)) !== false) && ($recordCount <= $this->recordLimit)) {
                $recordCount = $recordCount + 1;
                
                $pieces = explode(",", $line);

                $record = new Record();
                $record->playerID =  $pieces[0];
                $record->yearID =  $pieces[1];
                $record->stint =  $pieces[2];
                $record->teamID =  $pieces[3];
                $record->lgID =  $pieces[4];
                $record->G =  $pieces[5];
                $record->AB =  $pieces[6];
                $record->R =  $pieces[7];
                $record->H =  $pieces[8];

                $resultSet->addItem($record, $recordCount);
            }
        
            fclose($handle);

            return $this->RecordsToXML($resultSet, $StartTime);

        } else {
            echo "Data File not found!";
        } 
    }

    private function RecordsToXML($resultSet, $StartTime) {
        $xmlString = '<?xml version="1.0" encoding="UTF-8" standalone="no" ?>';
        $xmlString .= '<ReturnModel xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://schemas.datacontract.org/2004/07/CSharpWebService.Models">';
        $xmlString .= '<ArrayOfRecords>';
        
        $recordCount = $resultSet->length();

        for ($i = 1; $i <= $recordCount ; $i++) {
            $record = $resultSet->getItem($i);
            
            $xmlString .= '<record>';
            $xmlString .= '<playerID>' . $record->playerID . '</playerID>';
            $xmlString .= '<yearID>' . $record->yearID . '</yearID>';
            $xmlString .= '<stint>' . $record->stint . '</stint>';
            $xmlString .= '<teamID>' . $record->teamID . '</teamID>';
            $xmlString .= '<lgID>' . $record->lgID . '</lgID>';
            $xmlString .= '<G>' . $record->G . '</G>';
            $xmlString .= '<AB>' . $record->AB . '</AB>';
            $xmlString .= '<R>' . $record->R . '</R>';
            $xmlString .= '<H>' . $record->H . '</H>';
            $xmlString .= '</record>';
        }

        $xmlString .= '</ArrayOfRecords>';
        $xmlString .= '<ProcessingTime>' . round((microtime(true) - $StartTime) * 1000, 4)  . '</ProcessingTime>';
        $xmlString .= '</ReturnModel>';
        
        return $xmlString;
    }
    
 }
 ?>