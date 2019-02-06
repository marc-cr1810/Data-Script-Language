style {
    window {
        title: "Test Title"
        size: [
            width: 600,
            height: 400
        ]
        resizeable: true
        color.background: 255, 255, 127
    }

    dialog {
        title: "${style.window.title} Dialog"
        size: [
            width: <?${style.window.size("width")} / 2>,
            height: <?${style.window.size("height")} / 2>
        ]
        show: false
        color.background: ${style.window.color.background->B}, ${style.window.color.background->G}, ${style.window.color.background->R}
    }
}

objects {
    poly {
        vertices: [
            vert1: [x: 15,                                          y: 15],
            vert2: [x: <?${style.window.size("width")} - (15 * 2)>, y: 15],
            vert3: [x: <?${style.window.size("width")} - (15 * 2)>, y: <?${style.window.size("height")} - (15 * 2)>],
            vert4: [x: 15,                                          y: <?${style.window.size("height")} - (15 * 2)>]
        ]
        color: 127, 255, 127
    }
}

lang {
    console.messages: [
        first: "First Message",
        second: "Second Message",
        third: "Third Message"
    ]
}