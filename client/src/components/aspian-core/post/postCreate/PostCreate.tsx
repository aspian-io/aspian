import React, { Fragment, useContext } from 'react';
import CKEditor from '@ckeditor/ckeditor5-react';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
// Import translations for the German language.
import '@ckeditor/ckeditor5-build-classic/build/translations/fa';
import { CoreRootStoreContext } from '../../../../app/stores/aspian-core/CoreRootStore';
import { observer } from 'mobx-react-lite';
import { Row, Col } from 'antd';

const PostCreate = () => {
  const coreRootStore = useContext(CoreRootStoreContext);
  const { lang, storeCkEditorInstance } = coreRootStore.localeStore;

  const config = {
    language: lang,
  };

  return (
    <Fragment>
      <Row>
        <Col xs={24} sm={24} md={16}>
          <h2>Using CKEditor 5 build in React</h2>
          <div id="addPostCkEditor">
            <CKEditor
              config={config}
              editor={ClassicEditor}
              data="<p>Hello from CKEditor 5!</p>"
              onInit={(editor: any) => {
                // You can store the "editor" and use when it is needed.
                console.log('Editor is ready to use!', editor);
                storeCkEditorInstance(editor);
              }}
              onChange={(event: any, editor: any) => {
                const data = editor.getData();
                console.log({ event, editor, data });
              }}
              onBlur={(event: any, editor: any) => {
                console.log('Blur.', editor);
              }}
              onFocus={(event: any, editor: any) => {
                console.log('Focus.', editor);
              }}
            />
          </div>
        </Col>
      </Row>
    </Fragment>
  );
};

export default observer(PostCreate);
