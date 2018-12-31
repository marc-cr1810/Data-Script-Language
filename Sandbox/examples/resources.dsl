style {
    window {
        title: "Test Title"
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