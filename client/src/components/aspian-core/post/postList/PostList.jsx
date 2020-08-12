import React, {useState } from 'react';
import { Table, Button, Space, Tooltip } from 'antd';
import {
  EditFilled,
  DeleteFilled,
  ClockCircleFilled,
} from '@ant-design/icons';
import '../../../../scss/aspian-core/pages/posts/all-posts/_all-posts.scss';

const PostList = () => {
  //const postStore = useContext(PostStore);
  const [selectedRowKeys, setSelectedRowKeys] = useState([]);

  const onSelectChange = (selectedRowKeys) => {
    console.log('selectedRowKeys changed: ', selectedRowKeys);
    setSelectedRowKeys(selectedRowKeys);
  };

  const rowSelection = {
    selectedRowKeys,
    onChange: onSelectChange,
    selections: [
      Table.SELECTION_ALL,
      Table.SELECTION_INVERT,
      {
        key: 'odd',
        text: 'Select Odd Row',
        onSelect: (changableRowKeys) => {
          let newSelectedRowKeys = [];
          newSelectedRowKeys = changableRowKeys.filter((key, index) => {
            if (index % 2 !== 0) {
              return false;
            }
            return true;
          });
          setSelectedRowKeys(newSelectedRowKeys);
        },
      },
      {
        key: 'even',
        text: 'Select Even Row',
        onSelect: (changableRowKeys) => {
          let newSelectedRowKeys = [];
          newSelectedRowKeys = changableRowKeys.filter((key, index) => {
            if (index % 2 !== 0) {
              return true;
            }
            return false;
          });
          setSelectedRowKeys(newSelectedRowKeys);
        },
      },
    ],
  };

  const columns = [
    {
      title: 'Title',
      width: 200,
      dataIndex: 'title',
      fixed: window.innerWidth > 576 ? 'left' : undefined,
      ellipsis: true,
    },
    {
      title: 'Category',
      width: 200,
      dataIndex: 'postCategory',
      ellipsis: true,
    },
    {
      title: 'Status',
      width: 100,
      dataIndex: 'postStatus',
    },
    {
      title: 'Attachments',
      width: 130,
      dataIndex: 'postAttachments',
      align: 'center',
    },
    {
      title: 'Comment Allowed',
      width: 200,
      dataIndex: 'commentAllowed',
      align: 'center',
    },
    {
      title: 'View Count',
      width: 200,
      dataIndex: 'viewCount',
      align: 'center',
    },
    {
      title: 'Pinned',
      width: 100,
      dataIndex: 'pinned',
      align: 'center',
    },
    {
      title: 'Histories',
      width: 100,
      dataIndex: 'postHistories',
      align: 'center',
    },
    {
      title: 'Comments',
      width: 120,
      dataIndex: 'comments',
      align: 'center',
    },
    {
      title: 'Child Posts',
      width: 150,
      dataIndex: 'childPosts',
      align: 'center',
    },
    {
      title: 'Created At',
      width: 200,
      dataIndex: 'createdAt',
    },
    {
      title: 'Created By',
      width: 150,
      dataIndex: 'createdBy',
    },
    {
      title: 'Modified At',
      width: 150,
      dataIndex: 'modifiedAt',
    },
    {
      title: 'Modified By',
      width: 150,
      dataIndex: 'modifiedBy',
    },
    {
      title: 'User Agent',
      width: 150,
      dataIndex: 'userAgent',
      ellipsis: true,
    },
    {
      title: 'IP Address',
      width: 150,
      dataIndex: 'userIPAddress',
    },
    {
      title: 'Actions',
      key: 'operation',
      fixed: window.innerWidth > 576 ? 'right' : undefined,
      width: 150,
      align: "center",
      render: () => (
        <Space>
          <Tooltip title="Edit Post" color="gray">
            <Button
              type="link"
              size="middle"
              icon={<EditFilled />}
              className="text warning-color"
            />
          </Tooltip>
          <Tooltip title="Delete Post" color="gray">
            <Button type="link" size="middle" icon={<DeleteFilled />} danger />
          </Tooltip>
          <Tooltip title="Post History" color="gray">
            <Button type="link" size="middle" icon={<ClockCircleFilled />} />
          </Tooltip>
        </Space>
      ),
    },
  ];

  let data = [];

//   useEffect(() => {
//     postStore.loadPosts();
//   }, [postStore]);

//   postStore.postsByDate.forEach((post, i) => {
//     data.push({
//       key: i,
//       title: post.title,
//       postCategory: post.taxonomyPosts.map((taxonomyPost: ITaxonomyPost) =>
//         taxonomyPost.taxonomy.type === TaxonomyTypeEnum.category
//           ? `${taxonomyPost.taxonomy.term.name} \n`
//           : null
//       ),
//       postStatus: post.postStatus,
//       postAttachments: 4,
//       commentAllowed: post.commentAllowed ? (
//         <CheckOutlined style={{ color: '#52c41a' }} />
//       ) : (
//         <CloseOutlined style={{ color: '#f5222d' }} />
//       ),
//       viewCount: post.viewCount,
//       pinned: post.isPinned ? (
//         <CheckOutlined style={{ color: '#52c41a' }} />
//       ) : (
//         <CloseOutlined style={{ color: '#f5222d' }} />
//       ),
//       postHistories: post.postHistories,
//       comments: post.comments,
//       childPosts: post.childPosts,
//       createdAt: moment(post.createdAt).format('YYYY-MM-DD HH:m:s'),
//       createdBy: post.createdBy?.userName,
//       modifiedAt: post.modifiedAt
//         ? moment(post.modifiedAt).format('YYYY-MM-DD HH:m:s')
//         : '',
//       modifiedBy: post.modifiedBy,
//       userAgent: post.userAgent,
//       userIPAddress: post.userIPAddress,
//     });
//   });

  return (
    <Table
      //loading={postStore.loadingInitial}
      rowSelection={rowSelection}
      columns={columns}
      dataSource={data}
      size="small"
      scroll={{ x: window.innerWidth-100, y: window.innerHeight-100 }}
    />
  );
};

export default PostList;
