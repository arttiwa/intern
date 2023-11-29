<script >
import PopUp from "./popup.vue"

export default {
  components: {
    PopUp,
  },
  mounted() {
    this.createTable()

  },
  name: "home-main",
  data() {
    return {
      table: [],
      load: false,
      editClick: false,
      data: {},


    }
  },
  methods: {
    createTable() {
      fetch('https://dis02.dexon-technology.com/api/dexon-exam/get-data-list')
        .then(res => res.json())
        .then(res => {
          this.table = res
          this.load = true
        });
    },
    openM1(item) {
      this.editClick = true;
      this.data = item
      console.log(this.data)
    },
    closeM1() {
      this.editClick = false
    },
    del(item) {
      fetch('https://dis02.dexon-technology.com/api/dexon-exam/delete-data?id=' + item.id,
        { method: 'DELETE' })
        .then(response => response.json())
        .then(del => {
          console.log(item.id);
          alert("Delete Complete")
          this.createTable()
        })
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
      return formattedDate;
    },
    searchTagNo(item){
      this.data = item
      console.log("start finding your input")
      console.log()
    },


  }
}

</script>













<template>
  <h1>Table about . . . </h1>
  <div class="searchTagNo">
    <input v-model="table.tag_no" 
          list="tagNo" 
          @input="searchTagNo(table)" 
          placeholder="Search for Tag No." 
          >
  </div>

  <datalist id="tagNo" >
    <option v-for="item in table" >{{ item.tag_no }}</option>
  </datalist>



  <div class="conTable">
    <b-table class="bTable" id="table" v-if="load">
      <tr>
        <th>Tag No</th>
        <th>Pipe Size</th>
        <th>Pipe Location</th>
        <th>Installation Date</th>
        <th>Inspection Interval</th>
        <!-- <th>Is Active</th> -->
        <th>Edit More</th>
      </tr>
      <tr v-for="item in table">
        <!-- <td>{{ item.id }}</td> -->
        <td v-if="item.is_active">{{ item.tag_no }}</td>
        <td v-if="item.is_active">{{ item.pipe_size }}</td>
        <td v-if="item.is_active">{{ item.pipe_location }}</td>
        <td v-if="item.is_active"> {{ formateDate(item.installation_date) }} </td>
        <td v-if="item.is_active">{{ item.inspection_interval }}</td>
        <!-- <td v-if="item.is_active">{{ item.is_active }}</td> -->
        <td v-if="item.is_active">
          <button class="editBut" id="editBut" v-on:click="openM1(item)"> Edit </button>
          <button class="delBut" id="delBut" v-on:click="del(item)">Delete</button>
        </td>
      </tr>
    </b-table>



    <PopUp v-show="editClick" :editData="data" @closeM1="closeM1" @refresh="createTable" @formDate="formateDate" />
  </div>


</template>












<style scoped lang="scss">



h1{
  text-align: center;
  background: #c38569;
}
.searchTagNo{
  margin: auto;
  text-align: center;
  background-color: #c38569;
  padding: 20px;
  input{
    width: 20%;
    height: 25px;
    border-radius: 5px;
    border: none;

  }
}
datalist{

}







.conTable {
  background: #848484;
  text-align: center;
  height: auto;
  width: 100%;
  padding: 50px;
  margin: 0px;

  .bTable {
    text-align: center;
    background: rgb(49, 94, 218);
    padding: 20px;
    width: 100%;
    height: 100%;

    tr {
      background: rgb(58, 207, 63);
      text-align: center;
      margin: auto;
      height: 100%;
      width: auto;

      th {
        border-bottom: 1px solid rgb(82, 78, 78);
        background: cadetblue;
        min-width: 10%;
        max-width: 100%;
        height: 100%;
        padding: 20px;
      }

      td {
        border-bottom: 1px solid rgb(61, 68, 71);
        background: teal;
        width: auto;
        height: 100%;


        .editBut {
          height: 30px;
          width: auto;
          border-radius: 5px;
          border: none;
          margin: 5px;
          background: #e8aa53;

          &:hover {
            background: #e0b67b;
            cursor: pointer;
          }
        }

        .delBut {
          height: 30px;
          width: auto;
          border-radius: 5px;
          border: none;
          margin: 5px;
          background: #c65019;

          &:hover {
            background: #c38569;
            cursor: pointer;
          }
        }
      }
    }
  }
}
</style>
