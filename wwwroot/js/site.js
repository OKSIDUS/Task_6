var connection = new signalR.HubConnectionBuilder().withUrl("/drawingHub").build();

connection.start().then(function () {
    console.log("SignalR Connected!");
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("ReceiveDrawing", function (data) {
    displayDrawing(data);
});



document.getElementById('saveDrawingButton').addEventListener('click', saveDrawing);

document.getElementById('uploadPicture').addEventListener('click', uploadPicture);

function uploadPicture() {
    fetch('/Home/GetDrawing?id=1') 
        .then(response => response.json())
        .then(data => {
            displayDrawing(data);
        })
        .catch(error => {
            console.error('Error uploading picture:', error);
        });
}

function displayDrawing(jsonData) {
    var layerData = JSON.parse(jsonData);

    var stage = new Konva.Stage({
        container: 'konva-container',
        width: window.innerWidth,
        height: window.innerHeight,
    });

    var layer = new Konva.Layer();
    stage.add(layer);
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
        })
        .catch(error => {
            console.error('Error saving drawing:', error);
        });

    connection.invoke("SendDrawing", JSON.stringify(layerData));
}

var stage = new Konva.Stage({
    container: 'konva-container',
    width: window.innerWidth,
    height: window.innerHeight,
});

var layer = new Konva.Layer();
stage.add(layer);

var isDrawing = false;
var currentLine;

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

