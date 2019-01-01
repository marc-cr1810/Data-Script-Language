style {
    window {
        title: "Test Title"
        size: [
            width: 600,
            height: 400
        ]
        color.background: <?200 + 55>, 255, 127
    }

    dialog {
        title: "${style.window.title} Dialog"
        size: [
            width: ${style.window.size("width")},
            height: 300
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