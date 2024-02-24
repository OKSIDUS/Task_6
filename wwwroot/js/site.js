var connection = new signalR.HubConnectionBuilder().withUrl("/drawingHub").build();
var stage = new Konva.Stage({
    container: 'konva-container',
    width: window.innerWidth,
    height: window.innerHeight,
});

var layer = new Konva.Layer();
stage.add(layer);

var isDrawing = false;
var currentLine;
connection.start().then(function () {
    console.log("SignalR Connected!");
}).catch(function (err) {
    return console.error(err.toString());
});

var id = document.getElementById('fieldId').value;
uploadPicture();

connection.on("ReceiveDrawing", function (data) {;
    displayDrawing(data, layer);
});



document.getElementById('saveDrawingButton').addEventListener('click', saveDrawing);

document.getElementById('uploadPicture').addEventListener('click', uploadPicture());

function uploadPicture() {
    fetch('/Home/GetDrawing?id='+id) 
        .then(response => response.json())
        .then(data => {
            if (typeof data === 'object') {
                data = JSON.stringify(data);
            }
            displayDrawing(data,layer);
        })
        .catch(error => {
            console.error('Error uploading picture:', error);
        });
}

function displayDrawing(jsonData, layer) {
    //var layer = new Konva.Layer();
    if (!layer) {
        console.error('Layer is undefined or null.');
        return;
    }

    var layerData = JSON.parse(jsonData);

    if (!Array.isArray(layerData)) {
        console.error('Layer data is not an array.');
        return;
    }
    
    layer.destroyChildren();

    layerData.forEach(function (item) {
        var node;

        if (item.type === 'Line') {
            node = new Konva.Line(item.attrs);
        }
        
        layer.add(node);
    });
    
    layer.batchDraw();
}
function saveDrawing() {
    var layerData = [];

    layer.children.forEach(function (node) {
        layerData.push({
            type: node.getClassName(),
            attrs: node.attrs,
        });
    });
    console.log("ok");
    fetch('/Home/SaveDrawing', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(layerData),
    })
        .then(response => response.json())
        .then(data => {
            console.log('Drawing saved successfully:', data);
            layer.batchDraw();
        })
        .catch(error => {
            console.error('Error saving drawing:', error);
        });

    connection.invoke("SendDrawing", JSON.stringify(layerData));
    

}




stage.on('mousedown touchstart', function (e) {
    isDrawing = true;
    var pos = stage.getPointerPosition();
    currentLine = new Konva.Line({
        points: [pos.x, pos.y],
        stroke: 'black',
        strokeWidth: 5,
        lineCap: 'round',
        lineJoin: 'round',
    });
    layer.add(currentLine);
});

stage.on('mousemove touchmove', function () {
    if (!isDrawing) {
        return;
    }
    var pos = stage.getPointerPosition();
    var newPoints = currentLine.points().concat([pos.x, pos.y]);
    currentLine.points(newPoints);
    layer.batchDraw();
});

stage.on('mouseup touchend', function () {
    isDrawing = false;
});

