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
        
        // Cater for header
        $this->recordLimit = $this->recordLimit + 1;    
        
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
        } else {
            echo "Data File not found!";
        } 
    }

 }
 ?>