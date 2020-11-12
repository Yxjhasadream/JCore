<template>
  <div class="">
    <span>orgintime</span>
    <input v-model="orgintime" />
    <br />
    <span>interval</span>
    <input v-model="interval" />

    <input
      type="radio"
      name="inter"
      v-for="item in timeOptions"
      v-bind:key="item"
      v-bind:value="item"
      v-bind:title="item"
      v-on:click="setInterval(item)"
    />

    <div>endtime{{ getEndTime() }}</div>
    <div>leastTime{{ getLeastTime() }}</div>
    <div>
      <input type="text" v-model="unix" />
      unixtoDate {{ getDateFromUnixTime() }}
    </div>
    <div>
      <input type="text" v-model="date" />
      datetoUnix{{ getUnixTimeFromDate() }}
    </div>

    <router-link class="back" to="/">返回首页</router-link>
  </div>
</template>
<script>
import moment from "moment";
export default {
  name: "timeUtils",
  data() {
    return {
      unix: "",
      date: "",
      orgintime: "",
      interval: "",
      timeOptions: [1.5, 2, 2.5, 3, 3.5, 4],
    };
  },
  methods: {
    getEndTime: function () {
      var self = this;
      var interval = self.interval;
      var orgintime = self.orgintime;
      if (!orgintime) {
        return "";
      }

      if (!interval) {
        return moment(self.orgintime, "HH:mm").add(9, "h").format("HH:mm");
      }
      var interval = interval ? interval : 0;
      var time = moment(self.orgintime, "HH:mm")
        .add(9, "h")
        .add(self.interval, "h");
      return time.format("HH:mm");
    },
    getLeastTime: function () {
      var self = this;
      var orgintime = self.orgintime;
      if (!orgintime) {
        return "";
      }
      var interval = interval ? interval : 0;
      var time = moment(self.orgintime, "HH:mm").add(10.5, "h");
      return time.format("HH:mm");
    },
    setInterval: function (val) {
      var self = this;
      if (!val) {
        return;
      }
      self.interval = val;
    },
    getUnixTimeFromDate: function () {
      var self = this;
      var date = self.date;
      if (!date) {
        return;
      }
      var unix = moment(date).format("X");
      var res = "(" + unix + ") 13 (" + moment(date).format("x") + ")";
      return res;
    },
    getDateFromUnixTime: function () {
      var self = this;
      // 1600120703  1600120680000
      if (!self.unix) {
        return;
      }
      var val = self.unix.toString().length === 13 ? self.unix : self.unix * 1000;
      var date = moment(parseInt(val)).format("YYYY-MM-DD HH:mm");
      return date;
    },
  },
};
</script>
<style scoped>
</style>