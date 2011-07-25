<?php
$url = 'http://api.kactoos.com/br/api/products/get-product-list/format/xml/oauth_consumer_key/{CONSUMER-KEY}/orderby/popular/limit/10/category/113';

$ch = curl_init($url);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_HEADER, 0);
$data = curl_exec($ch);

$doc = new SimpleXmlElement($data);

//echo "<pre>";
//print_r($doc);
//echo "</pre>";
?>
<html>
    <head>
        <title>LazyConsumer</title>
        <link rel="stylesheet" href="css/south-street/jquery-ui-1.8.14.custom.css" />
        <script src="js/jquery-1.5.1.min.js"></script>
        <script src="js/jquery-ui-1.8.14.custom.min.js"></script>
        <style type="text/css">
            #gallery { float: left; width: 75%; min-height: 12em; } * html #gallery { height: 12em; } /* IE6 */
            .gallery.custom-state-active { background: #eee; }
            .gallery li { float: left; width: 205px; padding: 0.4em; margin: 0 0.4em 0.4em 0; text-align: center; }
            .gallery li h5 { margin: 0 0 0.4em; cursor: move; }
            .gallery li a { float: right; }            
            .gallery li a.ui-icon-zoomin { float: left; }
            .gallery li img { width: 100%; cursor: move; }

            #trash { float: right; width: 20%; min-height: 38em; padding: 1%;} * html #trash { height: 38em; } /* IE6 */
            #trash h4 { line-height: 16px; margin: 0 0 0.4em; }
            #trash h4 .ui-icon { float: left; }
            #trash .gallery h5 { display: none; }
        </style>
        <script type="text/javascript" src="js/droppable.js"></script>
    </head>
    <body>           
        <div class="demo ui-widget ui-helper-clearfix">

            <ul id="gallery" class="gallery ui-helper-reset ui-helper-clearfix">

                <?php
                foreach ($doc->product as $data) {
                ?>
                    <li class="ui-widget-content ui-corner-tr" onMouseOut="style.background='#eee'" onMouseOver="style.background='#327E04'">
                        <img src="<?= $data->image ?>"  alt="The peaks of High Tatras"  />
                        <h5 class="ui-widget-header"><?= $data->product_name ?></h5>
                    </li>
                <?php
                }
                ?>
            </ul>

            <div id="trash" class="ui-widget-content ui-state-default">
                <h4 class="ui-widget-header"><span class="ui-icon ui-icon-trash">Trash</span>Sacola</h4>                
            </div>

        </div><!-- End demo -->

    </body>
</html>
