import React, { Fragment, useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { Row, Col, Typography, Form, Input, Button } from 'antd';
import FullTextEditorInit from '../../../../js-ts/aspian-core/vendors/tinymce5/FullEditor';

const PostCreate = () => {
  const { Title, Paragraph, Text } = Typography;
  const { TextArea } = Input;

  useEffect(() => {
    // Initialize the app
    FullTextEditorInit('#addPostEditor');
  }, []);

  return (
    <Fragment>
      <Form>
        <Row justify="space-between">
            <Typography>
              <Title level={4}>Add New Post</Title>
              <Paragraph ellipsis>
                <Text type="secondary">You can add new post in this page.</Text>
              </Paragraph>
            </Typography>
            <Form.Item>
              <Button type="primary" htmlType="submit">
                Publish
              </Button>
            </Form.Item>
        </Row>
        <Row>
          <Col xs={24} sm={18}>
            <Form.Item name="postContent">
              <TextArea id="addPostEditor" />
            </Form.Item>
          </Col>
          <Col xs={24} sm={6}></Col>
        </Row>
      </Form>
    </Fragment>
  );
};

export default observer(PostCreate);
