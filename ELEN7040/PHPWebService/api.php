<?php
require_once 'PerformanceAPI.class.php';


try {

    switch ($_REQUEST['method']) {
        case "TestPerformance":
            $API = new PerformanceAPI($_REQUEST['var1'], $_REQUEST['var2']);
            echo $API->processAPI();
        }

} catch (Exception $e) {
    echo json_encode(Array('error' => $e->getMessage()));
}
?>