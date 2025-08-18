"use strict";

Europa.Components.Chart = function (id) {
    this.id = id;
    this.type = "pie";
    this.label = [];
    this.backgroundColor = [];
    this.data = [];
    this.legendPosition = 'top';

    //TODO: organizar os atts single e multi.
    this.sort = 'asc';

  

    this.borderColor = 'rgba(0, 0, 0, 0)';
}


Europa.Components.Chart.prototype = {

    WithType: function (type) {
        this.type = type;
        return this;
    },

    WithContent: function (label, data, color) {
        this.label.push(label);
        this.backgroundColor.push(color);
        this.data.push(data);
        return this;
    },

    WithBorderColor: function (borderColor) {
        this.borderColor = borderColor
        return this;
    },

    WithSorting: function (sort) {
        this.sort = sort;
        return this;
    },

    WithLegendPosition: function (position) {
        this.legendPosition = position;
        return this;
    },

    //configuracao base para graficos simples. (ainda nao usado)
    ConfigureSingle: function () {
        var self = this;
        var chart = new Chart(self.id, {
            type: self.type,
            data: {
                labels: self.label,
                datasets: [{
                   
                    label: "teste2",
                    data: self.data,

                    backgroundColor: self.backgroundColor,
                    borderColor: self.borderColor
                }],
            },
            options: {
                //TODO: implementar options conforme necessidade.

            },

        });
        return chart;
    },

    //configuracao base para graficos com multiplas entradas.
    ConfigureMulti: function () {
        var self = this;
        var chart = new Chart(self.id, {
            type: self.type,
            data: {
                labels: self.label,

                datasets: [{
                    data: self.data,
                    backgroundColor: self.backgroundColor,
                    borderColor: self.borderColor
                }],
            },
            options: {
                //TODO: implementar options conforme necessidade.
                sort: self.sort,
                legend: {
                    position: self.legendPosition,
                },
                events:[],
            },
        });
        return chart;
    },
};