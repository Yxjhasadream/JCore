import Vue from 'vue'
import Router from 'vue-router'
import HelloWorld from '@/components/HelloWorld'
import SqlBuilder from '@/components/SqlBuilder'

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
    }
  ]
})
