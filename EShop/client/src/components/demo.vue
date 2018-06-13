<template>
    <div>
        <form action="/api/upload" method="post" enctype="multipart/form-data">
            <input type="file" name="files" multiple/>
            <input type="submit" value="提交"/>
        </form>
    </div>
</template>