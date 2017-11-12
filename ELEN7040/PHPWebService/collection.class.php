<?php
class Collection 
{
    private $items = array();

    public function addItem($obj, $key = null) {
        if ($key == null) {
            $this->items[] = $obj;
        }
        else {
            if (isset($this->items[$key])) {
                throw new KeyHasUseException("Key $key already in use.");
            }
            else {
                $this->items[$key] = $obj;
            }
        }
    }

    public function deleteItem($key) {
        if (isset($this->items[$key])) {
            unset($this->items[$key]);
        }
        else {
            throw new KeyInvalidException("Invalid key $key.");
        }
    }
    
    public function getItem($key) {
        if (isset($this->items[$key])) {
            return $this->items[$key];
        }
        else {
            throw new KeyInvalidException("Invalid key $key.");
        }
    }

    public function keys() {
        return array_keys($this->items);
    }
    
    public function length() {
        return count($this->items);
    }
    
    public function keyExists($key) {
        return isset($this->items[$key]);
    }    

    public function ToXML() {
        $xmlString = '<?xml version="1.0" encoding="UTF-8" standalone="no" ?>';
        $xmlString .= '<records>';
        
        $recordCount = $this->length();
        echo $recordCount;

        for ($i = 0; $i < $recordCount ; $i++) {
            $record = $this->getItem($i);
            
            $xmlString .= '<record>';
            $xmlString .= '<playerID>' . $record->playerID . '</playerID>';
            $xmlString .= '<yearID>' . $record->yearID . '</yearID>';
            $xmlString .= '<stint>' . $record->stint . '</stint>';
            $xmlString .= '<teamID>' . $record->teamID . '</teamID>';
            $xmlString .= '<lgID>' . $record->lgID . '</lgID>';
            $xmlString .= '<G>' . $record->G . '</G>';
            $xmlString .= '<AB>' . $record->ab . '</AB>';
            $xmlString .= '<R>' . $record->R . '</R>';
            $xmlString .= '<H>' . $record->H . '</H>';
            $xmlString .= '</record>';
        }

        $xmlString .= '</records>';

        return $xmlString;
    }
    
}