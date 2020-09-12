import React, { Fragment } from 'react';
import { observer } from 'mobx-react-lite';
import { Row, Col, Typography } from 'antd';

const PostCreate = () => {
  const { Title, Paragraph, Text } = Typography;

  return (
    <Fragment>
      <Row>
        <Col span={12}>
          <Typography>
            <Title level={4}>Add New Post</Title>
            <Paragraph ellipsis>
              <Text type="secondary">You can add new post in this page.</Text>
            </Paragraph>
          </Typography>
        </Col>
      </Row>
      <Row>
        <Col xs={24} sm={18}>
          <div id="addPostEditor"></div>
        </Col>
        <Col xs={24} sm={6}></Col>
      </Row>
    </Fragment>     
  );
};

export default observer(PostCreate);
