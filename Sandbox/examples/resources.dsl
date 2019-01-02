style {
    window {
        title: "Test Title"
        size: [
            width: 600,
            height: 400
        ]
        resizeable: false
        color.background: 255, 255, 127
    }

    dialog {
        title: "${style.window.title} Dialog"
        size: [
            width: <?${style.window.size("width")} / 2>,
            height: <?${style.window.size("height")} / 2>
        ]
        show: true
        color.background: ${style.window.color.background->B}, ${style.window.color.background->G}, ${style.window.color.background->R}
    }
}

lang {
    console.messages: [
        first: "First Message",
        second: "Second Message",
        third: "Third Message"
    ]
}