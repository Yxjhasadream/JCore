<template>
  <div class="sqlBuilder">
    <div>
      <label>
        <input type="radio" name="optionsRadios" id="optionsRadios2" v-model="sqlType" value="1" />sqlserver
      </label>
      <label>
        <input type="radio" name="optionsRadios" id="optionsRadios1" v-model="sqlType" value="2" />mysql
      </label>
      <router-link class="back" to="/">返回首页</router-link>
    </div>
    <div>
      <div class="otption">
        <span>表名</span>
        <input type="text" v-model="table" />
      </div>
      <div class="otption">
        <span>字段名</span>
        <input type="text" v-model="column" />
      </div>
      <div class="otption">
        <span>字段类型</span>
        <input type="text" v-model="datatype" />
      </div>
      <div class="otption">
        <span>允许为空</span>
        <input type="checkbox" v-model="allownull" />
      </div>
      <div class="otption">
        <span>默认值</span>
        <input type="text" v-model="defval" />
      </div>
      <div class="otption">
        <span>注释</span>
        <input type="text" v-model="des" />
      </div>
    </div>
    <!--sqlsever -->
    <pre v-show="sqlType==='1'">
<h2>添加列与注释</h2>
ALTER TABLE {{table}} ADD {{column}} {{datatype}}  {{allownull?'':'NOT NULL'}} {{defval.length>0?'CONSTRAINT DF_'+table+'_'+column+ 'DEFAULT '+defval:'' }}
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{{des}}' ,
@level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{{table}}',
@level2type=N'COLUMN',@level2name=N'{{column}}'

<h2>修改注释</h2>
EXEC sp_updateextendedproperty @name=N'MS_Description', 
@value=N'{{des}}', @level0type=N'SCHEMA', @level0name=N'dbo',@level1type=N'TABLE',
@level1name=N'{{table}}', @level2type=N'COLUMN',
@level2name=N'{{column}}'

<h2>修改列的属性</h2>
EXEC sp_updateextendedproperty @name=N'MS_Description', 
@value=N'{{des}}', @level0type=N'SCHEMA', @level0name=N'dbo',@level1type=N'TABLE',
@level1name=N'{{table}}', @level2type=N'COLUMN',
@level2name=N'{{column}}'
    </pre>
    <!--mysql -->
    <pre v-show="sqlType==='2'">
<h2>添加列与注释</h2>
alter table {{table}} add column {{column}} {{datatype}} {{allownull?'':'not null'}}  {{defval.length>0?"Default "+defval:''}}  {{des.length>0?"COMMENT '"+des+"'":''}};

<h2>修改注释</h2>
alter table {{table}} modify column {{column}} {{datatype}} {{allownull?'':'not null'}} {{defval.length>0?"Default "+defval:''}} comment '{{des}}';

<h2>修改列的属性</h2>
alter table {{table}} modify {{column}} {{datatype}};
    </pre>
  </div>
</template>

<script>
export default {
  name: "SqlBuilder",
  data() {
    return {
      table: "表名",
      defval: "默认值",
      allownull:false,
      column: "列名",
      des: "注释",
      datatype: "字段类型",
      msg: "动态生成sql",
      sqlType: "1",
    };
  },
};
</script>

<style scoped>
pre {
  height: 500px;
  text-align: left;
}
.back {
  position: relative;
  left: 500px;
}
.option {
  margin-top: 500px;
}
</style>
