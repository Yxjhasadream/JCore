import Vue from 'vue'
import Router from 'vue-router'
import HelloWorld from '@/components/HelloWorld'
import SqlBuilder from '@/components/SqlBuilder'
import select from '@/components/select'
import csharpsummary from '@/components/csharpsummary'
import timeUtils from '@/components/timeUtils'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'HelloWorld',
      component: HelloWorld
    },
    {
      path: '/tool/sqlbuilder',
      name: 'sql',
      component: SqlBuilder
    },
    {
      path: '/select',
      name: 'select',
      component: select
    },
    {
      path:'/tool/csharpsummary',
      name:"csharpsummary",
      component:csharpsummary
    },
    {
      path:'/tool/time',
      name:'timeUtils',
      component:timeUtils
    }
  ]
})
