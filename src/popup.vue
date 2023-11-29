<script>
export default {
    name: "popup",
    props: {
        editData: Object,

    },
    mounted() {
        this.tagNoEdit = this.editData.tag_no;
        this.pipeSizeEdit = this.editData.pipe_size;
        this.pipeLoEdit = this.editData.pipe_location;
        this.inIntEdit = this.editData.inspection_interval;
        this.inDateEdit = this.editData.installation_date ? this.editData.installation_date.substring(0, 10) : '';

    },
    data() {
        return {
            tagNoEdit: '',
            pipeSizeEdit: '',
            pipeLoEdit: '',
            inIntEdit: '',
            inDateEdit: '',

        }
    },
    methods: {
        closeM1() {
            console.log("Close Edit Popup")
            this.$emit('closeM1')
        },
        formateDate(dateString) {
            const date = new Date(dateString);

            if (isNaN(date.getTime())) {
                console.error('Invalid date format');
                return;
            }

            const day = String(date.getDate()).padStart(2, '0');
            const month = String(date.getMonth() + 1).padStart(2, '0');
            const year = date.getFullYear();

            const formattedDate = `${day}/${month}/${year}`;
            console.log(formattedDate);
            return formattedDate;
        },

        saveEditData(data) {
            console.log("===================")
            console.log(data)
            console.log(this.editData.id)


            if (typeof this.tagNoEdit !== "number") {
                alert("Please input Tag No isn't number type");
                console.log(this.tagNoEdit)
                console.log(typeof this.tagNoEdit)

            }
            if (typeof this.pipeSizeEdit !== "number") {
                console.log(this.pipeSizeEdit)
                console.log(typeof this.pipeSizeEdit)
                alert("Please input Pipe Size isn't number type");
            }

            if (typeof this.tagNoEdit == "number" && typeof this.pipeSizeEdit == "number") {
                fetch('https://dis02.dexon-technology.com/api/dexon-exam/edit-data', {
                    method: 'PUT',
                    headers: {
                        'Content-type': 'application/json'
                    },
                    body: JSON.stringify({
                        "id": data.id,
                        "tag_no": this.tagNoEdit,
                        "pipe_size": this.pipeSizeEdit,
                        "pipe_location": this.pipeLoEdit,
                        "installation_date": this.inDateEdit,
                        "inspection_interval": this.inIntEdit,
                        "is_active": true
                    })
                }).then(res => {
                    if (!res.ok) {
                        throw new Error(`Error ${res.status}: ${res.statusText}`);
                    }
                    return res.json();
                }).then(res => {
                    console.log(data)
                    alert("Your Edit Data have saved")
                    this.$emit('refresh')
                    this.closeM1()

                })
                    .catch(error => {
                        console.error('Fetch Error:', error);
                        // Handle error: show an error message or perform other actions
                    });
            }
        },
    }
}
</script>





<template>
    <div id="conM1" class="conM1">
        <div class="editContent">
            <span class="closeEdit" v-on:click="closeM1()">&times;</span>
            <div class="conEdit">
                <h1>Edit Data</h1>
            </div>
            <div class="conEdit">
                <div class="TH">
                    <p>Tag No.</p>
                    <p>ssss</p>
                </div>

                <input  v-model.number="tagNoEdit" :placeholder="editData.tag_no">
                <input  v-model.number="pipeSizeEdit" :placeholder="editData.pipe_size">
                <input  v-model="pipeLoEdit" :placeholder="editData.pipe_location">
                <input v-if="editData.installation_date" type="date" v-model="inDateEdit"
                    :placeholder="editData.installation_date.substring(0, 10)">
                <input  v-model="inIntEdit" :placeholder="editData.inspection_interval">

            </div>
            <div class="">
                <input type="button" value="submit" v-on:click="saveEditData(editData)" class="editBut">
            </div>
            
        </div>

    </div>
</template>






<style scoped lang="scss">
.conM1 {
    background: brown;
    margin: auto;

    .editContent {
        background: blue;
        width: 70%;
        margin: auto;

        span {
            color: white;
            font-size: 20px;
        }

        .conEdit {
            background: violet;

            h1 {
                background: white;
            }

            p {
                display: inline;
                background: lawngreen;



            }

            input {
                background: rosybrown;
                margin: 5px;
                border: none;
                border-radius: 5px;
            }
        }
    }
}

.editBut {
    background: darkblue;
}
</style>