import React from 'react';
import { Result, Button } from 'antd';

const NetworkProblem = () => {
  return (
    <Result
      status="500"
      title="500"
      subTitle="Sorry, something went wrong. (Network error)"
      extra={
        <Button type="primary" href="/admin/dashboard">
          Back to dashboard
        </Button>
      }
    />
  );
};

export default NetworkProblem;
