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

lang {
    console.messages: [
        first: "First Message",
        second: "Second Message",
        third: "Third Message"
    ]
}