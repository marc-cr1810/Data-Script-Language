style {
    window {
        title: "Test Title"
        size: [
            width: 600,
            height: 400
        ]
        color.background: 255, 255, 127
    }

    dialog {
        title: "${style.window.title} Dialog"
        show: true
        color.background: ${style.window.color.background}
    }
}

objects {
    square {
        location: [
            x: 15,
            y: 15
        ]
        size: [
            width: <?${style.window.size("width")} - 15>,
            height: <?${style.window.size("height")} - 15>
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