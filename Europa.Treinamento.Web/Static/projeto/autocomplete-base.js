"use strict";

Europa.Components.AutoComplete = function () {
    this.prefix = "autocomplete_";
    this.param = "Nome";
    this.paramCallback = undefined;
    this.ajax = true;
    this.minimumInputLength = 2;
    this.placeholder = "";
    this.startsWith = false;
    this.parent = undefined;
    this.element = undefined;
    return this;
};

/* Builder Pattern Methods */
Europa.Components.AutoComplete.prototype.WithParamName = function (param) {
    this.param = param;
    return this;
};

Europa.Components.AutoComplete.prototype.SetStartsWith = function (allow) {
    this.startsWith = allow;
    return this;
};

Europa.Components.AutoComplete.prototype.WithAjax = function (param) {
    this.ajax = param;
    return this;
};

Europa.Components.AutoComplete.prototype.WithParent = function (parent) {
    this.parent = parent.startsWith("#") ? parent : "#" + parent;
    return this;
};

Europa.Components.AutoComplete.prototype.WithParamObjectCallback = function (param) {
    this.paramCallback = param;
    return this;
};

Europa.Components.AutoComplete.prototype.WithMinumunInputLength = function (minumunInputLength) {
    this.minumunInputLength = minumunInputLength;
    return this;
};

Europa.Components.AutoComplete.prototype.WithAction = function (action) {
    this.action = action;
    return this;
};

Europa.Components.AutoComplete.prototype.WithTargetSuffix = function (target) {
    this.id = this.prefix + target;
    return this;
};

Europa.Components.AutoComplete.prototype.WithPlaceholder = function (placeholder) {
    this.placeholder = placeholder;
    return this;
};


Europa.Components.AutoComplete.prototype.Configure = function () {
    var self = this;
    var options = {
        allowClear: true,
        placeholder: self.placeholder,
        escapeMarkup: function (markup) { return markup; },
        templateResult: this.FormatResponseSelection,
        templateSelection: this.FormatResponse
    };

    var ajax = {
        url: this.action,
        dataType: "json",
        delay: 200,
        data: function (params) {
            return self.Data(params);
        },
        processResults: function (data) {
            return self.ProcessResult(data);
        },
        cache: true
    };

    if (this.startsWith) {
        options['matcher'] = function (params, data) {
            params.term = params.term || '';
            if (data.text.toUpperCase().indexOf(params.term.toUpperCase()) == 0) {
                return data;
            }
            return false;
        };
    }

    if (this.ajax) {
        options['ajax'] = ajax;
        options['minimumInputLength'] = 1;
    }

    if (this.parent) {
        this.element = $("#" + this.id, self.parent);
    } else {
        this.element = $("#" + this.id);
    }

    this.element.select2(options);
    return this;
};

/* Default Implementation of methods */
Europa.Components.AutoComplete.prototype.ProcessResult = function(data) {
    var formattedResult = [];
    data.records.forEach(function(element) { formattedResult.push({ id: element.Id, text: element.Nome }); });
    return {
        results: formattedResult
    };
};

Europa.Components.AutoComplete.prototype.Data = function(params) {
    var data = {
        start: 0,
        pageSize: 10,
        filter: [
            {
                value: params.term,
                column: this.param,
                regex: true
            }
        ],
        order: [
            {
                value: "asc",
                column: this.param
            }
        ]
    };

    if (this.paramCallback) {
        var object = this.paramCallback();
        for (var i in object) {
            data.filter.push({
                value: object[i],
                column: i
            })
        }
    }
    return data;
};

Europa.Components.AutoComplete.prototype.FormatResponseSelection = function (model) {
    return model.Nome || model.text;
};

Europa.Components.AutoComplete.prototype.FormatResponse = function (model) {
    if (model.loading) return model.text;
    return "<option value='" + model.id + "'>" + model.text + "</option>";
}

Europa.Components.AutoComplete.prototype.SetValue = function (key, value) {
    this.element.html("<option value='" + key + "' selected>" + value + "</option>");
};

Europa.Components.AutoComplete.prototype.SetSelected = function (key) {
    this.element.val(key).trigger('change.select2');
};

Europa.Components.AutoComplete.prototype.Disable = function () {
    this.element.prop('disabled', true);
};

Europa.Components.AutoComplete.prototype.Enable = function () {
    this.element.prop('disabled', false);
};

Europa.Components.AutoComplete.prototype.Value = function () {
    return this.element.val();
};

Europa.Components.AutoComplete.prototype.Text = function () {
    return $("#" + this.id + " option:selected", this.parent).text();
};

Europa.Components.AutoComplete.prototype.Clean = function () {
    this.element.val([]);
    this.Configure();
};

Europa.Components.AutoComplete.prototype.ClearOptions = function () {
    this.element.empty();
    this.Configure();
};

Europa.Components.AutoComplete.AdjustWidth = function() {
    $(".select2-container").css("width", "100%");
};
