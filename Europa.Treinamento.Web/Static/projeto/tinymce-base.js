"use strict";

Europa.Components.TinyMCE = function () {
    this.selector = '';
    this.plugins = 'lists advlist link image';
    this.language = 'pt_BR';
    this.height = 200;
    this.relative_urls = false;
    this.remove_script_host = false;
    this.convert_urls = true;
    this.menubar = false;
    this.statusbar = false;
    this.toolbar = 'bold italic underline strikethrough | styleselect | alignleft aligncenter alignright alignjustify |' +
    ' bullist numlist | undo redo | link image';
    this.toolbar_items_size = 'small';
};

Europa.Components.TinyMCE.prototype.Configure = function () {
    tinymce.init(this);
    return this;
}

Europa.Components.TinyMCE.prototype.WithSelector = function (selector) {
    this.selector = selector;
    return this;
}

Europa.Components.TinyMCE.prototype.WithHeight = function (height) {
    this.height = height;
    return this;
}

Europa.Components.TinyMCE.prototype.WithMenuBar = function (menubar) {
    this.menubar = menubar;
    return this;
}

Europa.Components.TinyMCE.prototype.WithStatusBar = function (statusbar) {
    this.statusbar = statusbar;
    return this;
}

Europa.Components.TinyMCE.prototype.WithToolbarItemSize = function (toolbar_items_size) {
    this.toolbar_items_size = toolbar_items_size;
    return this;
}

Europa.Components.TinyMCE.prototype.AddToolbarSeparator = function () {
    this.toolbar = this.toolbar + ' | ';
    return this;
}

Europa.Components.TinyMCE.prototype.AddToolsToolbar = function (tool) {
    this.toolbar = this.toolbar + ' ' + tool;
    return this;
}