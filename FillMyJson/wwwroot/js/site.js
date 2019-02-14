// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
serverBus = new Vue();
new Vue({
    el: '#form',
    data: {
        FullName: '',
        Email: '',
        Comments: ''
    },
    methods: {
        ResetJson() {
            $.post("/Home/ResetJson")
                .done(() => {
                    serverBus.$emit('usersReset');
                })
                .fail((data) => {
                    console.error("Failed");
                    console.log(data);
                })
        },
        SubmitForm() {
            $.post("/Home/PostFormData", { User: this.$data })
                .done((data) => {
                    serverBus.$emit('userAdded');
                })
                .fail((data) => {
                    console.error("Failed");
                    console.log(data);
                })
        }
    }
});

new Vue({
    el: "#demo",
    template: '<ul class="list-group"><li class="list-group-item" v-for="user in data">{{user.fullName}} {{user.email}} {{user.comments}}</li></ul>',
    data: function () {
        return {
            data: 'no users found.'
        }
    },
    created() {
        this.updateUsers();
        serverBus.$on('userAdded', () => {
            this.updateUsers();
        });
        serverBus.$on('usersReset', () => {
            this.data = 'no users found';
        });
    },
    methods: {
        updateUsers: function () {
            fetch('/Home/GetUsers')
                .then(response => response.json())
                .then(data => {
                    this.data = data;
                });
        }
    }
})
