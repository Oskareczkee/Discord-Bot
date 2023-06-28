var ModalWindow = (function () {
    function ModalWindow($content) {
        this.modalBackground = $('<div class="modal-background"/>');
        this.modal = $('<div class="container modal-content"/>');
        this.closeButton = $('<div class="modal-close-btn" />');
        this.closeButton.on('click', buttonClickEvent);

        $.get($content, function (data) {
            var content = $('<div class="overflow-scroll modal-hide-scroll"></div>').append(data);
            content.find(".modal-close").on('click', buttonClickEvent);
            $(".modal-content").append(content);
        });


        this.modal.append(this.content, this.closeButton);
        this.modalBackground.append(this.modal);
    }

    ModalWindow.prototype.open = function () {
        this.modalBackground.appendTo('body');
        this.modalBackground.animate({ opacity: 1 }, 400);
    };

    ModalWindow.prototype.close = function () {
        $(this.closeButton).trigger('click');
    };

    let buttonClickEvent = function () {
        $(".modal-background").fadeOut({
            duration: 300, complete: function () {
                this.remove();
            }
        });
    };

    return ModalWindow;
}());

