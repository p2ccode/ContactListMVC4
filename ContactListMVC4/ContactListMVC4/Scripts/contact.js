
var API_URL = "http://localhost:59139/api/contact/";

function contact(Id, FirstName, LastName, City, Phone) {
    this.Id = ko.observable(Id);
    this.FirstName = ko.observable(FirstName);
    this.LastName = ko.observable(LastName);
    this.Name = ko.computed(function () {
        return this.FirstName() + ' ' + this.LastName();
    }, this);
    this.City = ko.observable(City);
    this.Phone = ko.observable(Phone);

    this.isEditing = ko.observable(false);

    this.startEdit = function (event) {
        this.isEditing(true);
    }

    this.updateContact = function (contact) {
        var self = this;
        var conj = ko.toJS(contact);
        var json = JSON.stringify(conj);
        var id = contact.Id();

        $.ajax({
            url: API_URL + id,
            cache: false,
            type: 'PUT',
            contentType: 'application/json; charset=utf-8',
            data: json,
            success: function () { self.isEditing(false); }
        });
    }
}

function LoadPeopleFromServer(_this) {
    $.get(
                API_URL,
                function (data) {
                    var results = ko.observableArray();
                    ko.mapping.fromJS(data, {}, results);
                    for (var i = 0; i < results().length; i++) {
                        _this.Contacts.push(new contact(results()[i].Id(), results()[i].FirstName(), results()[i].LastName(), results()[i].City(), results()[i].Phone()));
                    };
                },
                'json'
            )
}

var viewModel = function () {

    self = this;
    self.FirstName = ko.observable();
    self.LastName = ko.observable();
    self.City = ko.observable();
    self.Phone = ko.observable();
    self.Contacts = ko.observableArray();

    LoadPeopleFromServer(self);
    //to remove contact
    self.removeContact = function (contact) {
        var conj = ko.toJS(contact);
        var json = JSON.stringify(conj);
        var Id = contact.Id();
        $.ajax({
            url: API_URL + Id,
            cache: false,
            type: 'DELETE',
            contentType: 'application/json; charset=utf-8',
            data: '',
            success: function () {
                self.Contacts.remove(contact);
            }
        });
    }
    //add new contact
    self.addContact = function () {

        var con = new contact(0, self.FirstName(), self.LastName(), self.City(), self.Phone());

        var conj = ko.toJS(con);
        var json = JSON.stringify(conj);

        $.ajax({
            url: API_URL,
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: json,
            statusCode: {
                201 /*Created*/: function (data) {
                    self.Contacts.push(new contact(data.Id, data.FirstName, data.LastName, data.City, data.Phone));
                    self.FirstName("");
                    self.LastName("");
                    self.City("");
                    self.Phone("");
                }
            }
        });

    }
}

ko.applyBindings(new viewModel());