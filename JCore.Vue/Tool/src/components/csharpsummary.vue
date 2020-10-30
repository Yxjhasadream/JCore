<template>
    <div class="">
       <textarea class="main" v-model="result" />
       <router-link class="back" to="/">返回首页</router-link>
       <textarea class="main" v-bind:value="getSummary()"></textarea>
    </div>
</template>
<script>
export default {
    name:"csharpsummary",
    data(){
        return{
            result:""
        }
    },
    methods:{
        getSummary:function() {
            var self=this;
            return self.result.replace(/public/g,"/// <summary>\r\n/// \r\n/// </summary>\r\npublic");
        }
    }
}
</script>
<style scoped>
.main{
height: 300px;
width: 500px;
}
</style>