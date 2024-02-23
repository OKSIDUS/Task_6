var stage = new Konva.Stage({
    container: 'konva-container',
    width: window.innerWidth,
    height: window.innerHeight,
    draggable: true,
    fill: 'black',
});

var layer = new Konva.Layer();

stage.add(layer);

var rect = new Konva.Rect({
    x: 50,
    y: 50,
    width: 100,
    height: 50,
    fill: 'blue',
});

layer.add(rect);

layer.draw();